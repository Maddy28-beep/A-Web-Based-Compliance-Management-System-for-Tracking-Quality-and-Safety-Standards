using System.Threading.RateLimiting;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FurniComply.Infrastructure;
using FurniComply.Infrastructure.Identity;
using FurniComply.Infrastructure.Persistence;
using FurniComply.Infrastructure.Services;
using FurniComply.Web;
using FurniComply.Web.Configuration;
using FurniComply.Web.Services;
using FurniComply.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using QuestPDF.Infrastructure;

DotEnvBootstrap.LoadIfPresent();

var builder = WebApplication.CreateBuilder(args);

EnvironmentVariablesConfiguration.Apply(builder.Configuration);

static string ResolveSharedDataProtectionKeysPath(string contentRootPath)
{
    var configuredPath = Environment.GetEnvironmentVariable("APP_DATA_PROTECTION_KEYS_PATH");
    if (!string.IsNullOrWhiteSpace(configuredPath))
    {
        return configuredPath;
    }

    var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    if (!string.IsNullOrWhiteSpace(localAppData))
    {
        return Path.Combine(localAppData, "FurniComply", "DataProtectionKeys");
    }

    return Path.Combine(contentRootPath, "App_Data", "DataProtectionKeys");
}

static void AddRolePolicy(AuthorizationOptions options, string policyName, params string[] roles)
{
    options.AddPolicy(policyName, policy => policy.RequireRole(roles));
}

if (FurniComplyEnvironments.IsCapstoneOrDevelopment(builder.Environment))
{
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Logging.AddDebug();
}

builder.Services.AddScoped<SupplierPerformanceService>();
builder.Services.AddScoped<ISupplierPerformanceService>(sp => sp.GetRequiredService<SupplierPerformanceService>());
builder.Services.AddScoped<ISupplierComplianceScoreService, SupplierComplianceScoreService>();
builder.Services.AddScoped<IAssignmentNotificationService, AssignmentEmailService>();
builder.Services.AddScoped<IPasswordHistoryService, PasswordHistoryService>();
builder.Services.AddScoped<IPasswordValidator<ApplicationUser>, CustomPasswordValidator>();
builder.Services.AddScoped<IProcurementService, ProcurementService>();
builder.Services.AddScoped<ISupplierManagementService, SupplierManagementService>();
builder.Services.AddScoped<AuthorizationPolicyHelper>();
builder.Services.AddSingleton<SmtpMailHealthService>();
builder.Services.AddScoped<IPasswordResetEmailSender, PasswordResetEmailSender>();
builder.Services.AddScoped<IReCaptchaService, ReCaptchaService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<SensitiveDataProtectionService>();
builder.Services.AddScoped<QRCodeHelper>();
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("Login", config =>
    {
        config.Window = TimeSpan.FromMinutes(1);
        config.PermitLimit = 10;
        config.QueueLimit = 0;
    });
    options.OnRejected = async (context, _) =>
    {
        if (context.HttpContext.Request.Path.StartsWithSegments("/Account/Login", StringComparison.OrdinalIgnoreCase))
        {
            context.HttpContext.Response.Redirect("/Account/Login?rateLimit=1");
        }
        else
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.");
        }
    };
});
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
var dataProtectionKeysPath = ResolveSharedDataProtectionKeysPath(builder.Environment.ContentRootPath);
Directory.CreateDirectory(dataProtectionKeysPath);
builder.Services
    .AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeysPath))
    .SetApplicationName("FurniComply");
builder.Services.Configure<PasswordHasherOptions>(options =>
{
    options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
    options.IterationCount = 210_000;
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 12;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
        options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
    })
    .AddEntityFrameworkStores<FurniComply.Infrastructure.Persistence.AppDbContext>()
    .AddDefaultTokenProviders();

var idleLogoutSeconds = builder.Configuration.GetValue("App:IdleLogoutSeconds", 0);
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "FurniComply.Auth";
    options.ExpireTimeSpan = idleLogoutSeconds > 0
        ? TimeSpan.FromSeconds(idleLogoutSeconds)
        : TimeSpan.FromMinutes(30);
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(5);
});

try
{
    QuestPDF.Settings.License = LicenseType.Community;
}
catch (Exception ex)
{
    Console.WriteLine($"QuestPDF initialization skipped: {ex.Message}");
}

builder.Services.AddAuthorization(options =>
{
    var superAdmin = FurniComply.Infrastructure.Identity.RoleNames.SuperAdmin;
    var admin = FurniComply.Infrastructure.Identity.RoleNames.Admin;
    var complianceManager = FurniComply.Infrastructure.Identity.RoleNames.ComplianceManager;
    var departmentHead = FurniComply.Infrastructure.Identity.RoleNames.DepartmentHead;
    var auditor = FurniComply.Infrastructure.Identity.RoleNames.Auditor;
    var procurement = FurniComply.Infrastructure.Identity.RoleNames.Procurement;

    AddRolePolicy(options, "System.Admin", superAdmin, admin);
    AddRolePolicy(options, "Suppliers.Override", superAdmin);
    AddRolePolicy(options, "Policies.Read", superAdmin, admin, complianceManager, departmentHead, auditor);
    AddRolePolicy(options, "Policies.Write", superAdmin, admin, complianceManager, departmentHead);
    AddRolePolicy(options, "Policies.Approve", superAdmin, admin);
    AddRolePolicy(options, "Compliance.Read", superAdmin, admin, complianceManager, departmentHead, auditor);
    AddRolePolicy(options, "Compliance.Write", superAdmin, admin, complianceManager, departmentHead);
    AddRolePolicy(options, "Compliance.Verify", superAdmin, admin, complianceManager);
    AddRolePolicy(options, "Reports.Read", superAdmin, admin, complianceManager, departmentHead, auditor);
    AddRolePolicy(options, "Reports.Write", superAdmin, admin, complianceManager);
    AddRolePolicy(options, "Reports.Approve", superAdmin, admin, complianceManager);
    AddRolePolicy(options, "Reports.ApproveOverride", superAdmin);
    AddRolePolicy(options, "Reports.Submit", superAdmin, admin, complianceManager);
    AddRolePolicy(options, "Reports.Export", superAdmin, admin, complianceManager);
    AddRolePolicy(options, "Procurement.Read", superAdmin, admin, complianceManager, procurement);
    AddRolePolicy(options, "Procurement.Write", superAdmin, admin, procurement);
    AddRolePolicy(options, "Procurement.Approve", superAdmin, admin);
    AddRolePolicy(options, "Procurement.PlaceOrder", superAdmin, admin, complianceManager);
    AddRolePolicy(options, "Suppliers.Approve", superAdmin, admin);
    AddRolePolicy(options, "Audit.Read", superAdmin, admin, complianceManager, departmentHead, auditor, procurement);
    AddRolePolicy(options, "Audit.Export", superAdmin, admin, complianceManager);

    AddRolePolicy(options, "Admin", superAdmin, admin);
});

var app = builder.Build();

if (await TryRunDevelopmentMaintenanceCliAsync(app, args))
    return;

var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
var usesSqlite = !string.IsNullOrWhiteSpace(defaultConnection) &&
    defaultConnection.TrimStart().StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase);
if (!string.IsNullOrWhiteSpace(defaultConnection) &&
    defaultConnection.TrimStart().StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
{
    var dataDir = Path.Combine(app.Environment.ContentRootPath, "App_Data");
    Directory.CreateDirectory(dataDir);
}

var seedIdentityOnStartup = string.Equals(
    builder.Configuration["SeedIdentityOnStartup"],
    "true",
    StringComparison.OrdinalIgnoreCase);
var seedOnStartup = string.Equals(
    builder.Configuration["SeedDataOnStartup"],
    "true",
    StringComparison.OrdinalIgnoreCase);
var seedLiveScenariosOnStartup = string.Equals(
    builder.Configuration["SeedLiveScenariosOnStartup"],
    "true",
    StringComparison.OrdinalIgnoreCase);

if (app.Environment.IsDevelopment() && seedIdentityOnStartup)
{
    await IdentitySeeder.SeedAsync(app.Services);
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (usesSqlite)
    {
        var connection = dbContext.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

        var existingUserColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        using (var userColumnCommand = connection.CreateCommand())
        {
            userColumnCommand.CommandText = "PRAGMA table_info(AspNetUsers);";
            using var reader = await userColumnCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var columnName = reader["name"]?.ToString();
                if (!string.IsNullOrWhiteSpace(columnName))
                {
                    existingUserColumns.Add(columnName);
                }
            }
        }

        var missingUserColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["IsBackupAccount"] = "ALTER TABLE AspNetUsers ADD COLUMN IsBackupAccount INTEGER NOT NULL DEFAULT 0;",
            ["IsHidden"] = "ALTER TABLE AspNetUsers ADD COLUMN IsHidden INTEGER NOT NULL DEFAULT 0;",
            ["IsSystemAccount"] = "ALTER TABLE AspNetUsers ADD COLUMN IsSystemAccount INTEGER NOT NULL DEFAULT 0;",
            ["IsActive"] = "ALTER TABLE AspNetUsers ADD COLUMN IsActive INTEGER NOT NULL DEFAULT 1;",
            ["CreatedAtUtc"] = "ALTER TABLE AspNetUsers ADD COLUMN CreatedAtUtc TEXT NOT NULL DEFAULT '';"
        };

        foreach (var pair in missingUserColumns)
        {
            if (existingUserColumns.Contains(pair.Key))
            {
                continue;
            }

            using var alterUserCommand = connection.CreateCommand();
            alterUserCommand.CommandText = pair.Value;
            await alterUserCommand.ExecuteNonQueryAsync();
        }

        var hasPhoneNumber = false;
        using (var columnCommand = connection.CreateCommand())
        {
            columnCommand.CommandText = "PRAGMA table_info(Suppliers);";
            using var reader = await columnCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                if (string.Equals(reader["name"]?.ToString(), "PhoneNumber", StringComparison.OrdinalIgnoreCase))
                {
                    hasPhoneNumber = true;
                    break;
                }
            }
        }

        if (!hasPhoneNumber)
        {
            using var alterCommand = connection.CreateCommand();
            alterCommand.CommandText = "ALTER TABLE Suppliers ADD COLUMN PhoneNumber TEXT;";
            await alterCommand.ExecuteNonQueryAsync();
        }

        var columnsToUpdate = new[]
        {
            "Address", "Certifications", "Code", "ContactEmail", "Name", "ApprovedBy",
            "OverrideReason", "OverriddenBy", "OverrideRequestReason", "RequestedBy", "PhoneNumber"
        };

        foreach (var column in columnsToUpdate)
        {
            using var updateCommand = connection.CreateCommand();
            updateCommand.CommandText = $"UPDATE Suppliers SET {column} = '' WHERE {column} IS NULL;";
            try
            {
                await updateCommand.ExecuteNonQueryAsync();
            }
            catch
            {
            }
        }

        if (connection.State == System.Data.ConnectionState.Open)
            await connection.CloseAsync();
    }
    else
    {
        try
        {
            await dbContext.Suppliers.FirstOrDefaultAsync();
        }
        catch (Exception ex) when (ex.Message.Contains("no such column") || ex.Message.Contains("PhoneNumber"))
        {
            var connection = dbContext.Database.GetDbConnection();
            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "ALTER TABLE Suppliers ADD COLUMN PhoneNumber TEXT;";
                await command.ExecuteNonQueryAsync();
            }
            if (connection.State == System.Data.ConnectionState.Open)
                await connection.CloseAsync();

            await dbContext.Database.ExecuteSqlRawAsync(
                "UPDATE Suppliers SET PhoneNumber = '' WHERE PhoneNumber IS NULL");
        }
        catch (Exception ex) when (ex.Message.Contains("NULL at ordinal") || ex.Message.Contains("IsDBNull"))
        {
            var connection = dbContext.Database.GetDbConnection();
            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            var columnsToUpdate = new[] { "Address", "Certifications", "Code", "ContactEmail", "Name", "ApprovedBy", "OverrideReason", "OverriddenBy", "OverrideRequestReason", "RequestedBy" };

            foreach (var column in columnsToUpdate)
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"UPDATE Suppliers SET {column} = '' WHERE {column} IS NULL;";
                    try
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                    catch
                    {
                    }
                }
            }

            if (connection.State == System.Data.ConnectionState.Open)
                await connection.CloseAsync();
        }
    }
}

if (seedOnStartup)
{
    await FurniComply.Infrastructure.Persistence.DemoDataSeeder.SeedAsync(
        app.Services,
        seedLiveScenariosOnStartup ? 60 : 25,
        seedLiveScenariosOnStartup);
}

app.UseExceptionHandler("/Home/Error");

app.UseRateLimiter();

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/uploads", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        return;
    }

    await next();
});

app.UseStaticFiles();

app.UseRouting();

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    context.Response.Headers["Content-Security-Policy"] = "default-src 'self'; script-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net https://cdnjs.cloudflare.com https://www.google.com https://www.gstatic.com; style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://cdn.jsdelivr.net https://cdnjs.cloudflare.com; font-src 'self' https://fonts.gstatic.com https://cdnjs.cloudflare.com; img-src 'self' data: https:; frame-src https://www.google.com https://www.gstatic.com; connect-src 'self' https://api.openweathermap.org https://open.er-api.com https://date.nager.at";
    await next();
});

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/Identity/Account/Register", StringComparison.OrdinalIgnoreCase) ||
        context.Request.Path.StartsWithSegments("/Identity/Account/Login", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.Redirect("/Account/Login");
        return;
    }

    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var path = context.Request.Path;
    var isSensitiveAdminApi =
        path.StartsWithSegments("/api/Debug", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWithSegments("/api/BackupAdmin", StringComparison.OrdinalIgnoreCase);

    if (!isSensitiveAdminApi)
    {
        await next();
        return;
    }

    var isPrivilegedUser =
        context.User.IsInRole(RoleNames.SuperAdmin) ||
        context.User.IsInRole(RoleNames.Admin);

    if (!isPrivilegedUser)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        return;
    }

    await next();
});

app.UseStatusCodePages(context =>
{
    var httpContext = context.HttpContext;
    var requestPath = httpContext.Request.Path;

    if (httpContext.Response.StatusCode == StatusCodes.Status400BadRequest &&
        (requestPath.StartsWithSegments("/Account/Login", StringComparison.OrdinalIgnoreCase) ||
         requestPath.StartsWithSegments("/Home/Login", StringComparison.OrdinalIgnoreCase) ||
         requestPath.StartsWithSegments("/Account/ForgotPassword", StringComparison.OrdinalIgnoreCase) ||
         requestPath.StartsWithSegments("/Account/ResetPassword", StringComparison.OrdinalIgnoreCase)))
    {
        foreach (var cookieKey in httpContext.Request.Cookies.Keys)
        {
            if (cookieKey.StartsWith(".AspNetCore.Antiforgery", StringComparison.OrdinalIgnoreCase))
            {
                httpContext.Response.Cookies.Delete(cookieKey);
            }
        }

        httpContext.Response.Redirect("/Account/Login?retry=1");
    }

    return Task.CompletedTask;
});

app.Use(async (context, next) =>
{
    var path = context.Request.Path;
    if (path == "/")
    {
        context.Response.Redirect("/Home/Index");
        return;
    }

    if (context.User.Identity?.IsAuthenticated == true)
    {
        await next();
        return;
    }

    var pathNorm = (path.Value ?? string.Empty).TrimEnd('/');
    var endpoint = context.GetEndpoint();
    var allowAnonymous =
        endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null ||
        PublicAnonymousPathHelper.MatchesPublicPathPrefix(pathNorm);

    if (allowAnonymous)
    {
        await next();
        return;
    }

    if (path.StartsWithSegments("/Dashboard", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.Redirect("/Account/Login?blocked=dashboard&returnUrl=%2FDashboard");
        return;
    }

    context.Response.Redirect("/Account/Login");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    await BackupSuperAdminSeeder.SeedBackupSuperAdminAsync(scope.ServiceProvider, logger);
}

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var sensitiveDataProtection = scope.ServiceProvider.GetRequiredService<SensitiveDataProtectionService>();
    await sensitiveDataProtection.ProtectExistingSensitiveDataAsync();
    logger.LogInformation("Sensitive-data encryption check completed at startup.");
}

app.Run();

static async Task<bool> TryRunDevelopmentMaintenanceCliAsync(WebApplication app, string[] cliArgs)
{
    if (cliArgs.Length == 0)
        return false;

    var command = cliArgs[0];
    if (!string.Equals(command, "reset-password", StringComparison.OrdinalIgnoreCase) &&
        !string.Equals(command, "create-user", StringComparison.OrdinalIgnoreCase))
        return false;

    if (!app.Environment.IsDevelopment())
    {
        Console.Error.WriteLine($"{command} is only available when ASPNETCORE_ENVIRONMENT=Development.");
        Environment.ExitCode = 1;
        return true;
    }

    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    if (string.Equals(command, "reset-password", StringComparison.OrdinalIgnoreCase))
    {
        if (cliArgs.Length < 3)
        {
            Console.Error.WriteLine("Usage: dotnet run -- reset-password <email> <newPassword>");
            Environment.ExitCode = 1;
            return true;
        }

        var email = cliArgs[1].Trim();
        var newPassword = cliArgs[2];
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            Console.Error.WriteLine($"No user found for email: {email}");
            Environment.ExitCode = 1;
            return true;
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, token, newPassword);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                Console.Error.WriteLine(error.Description);
            Environment.ExitCode = 1;
            return true;
        }

        await userManager.SetLockoutEndDateAsync(user, null);
        await userManager.ResetAccessFailedCountAsync(user);
        Console.WriteLine($"Password updated for {email}. Sign in at /Account/Login with the new password.");
        return true;
    }

    if (cliArgs.Length < 4)
    {
        Console.Error.WriteLine("Usage: dotnet run -- create-user <email> <password> <role> [fullName]");
        Console.Error.WriteLine($"Roles: {string.Join(", ", RoleNames.All)}");
        Environment.ExitCode = 1;
        return true;
    }

    var newEmail = cliArgs[1].Trim();
    var password = cliArgs[2];
    var role = cliArgs[3].Trim();
    var fullName = cliArgs.Length > 4 ? string.Join(' ', cliArgs.Skip(4)) : newEmail;

    if (!RoleNames.All.Contains(role, StringComparer.OrdinalIgnoreCase))
    {
        Console.Error.WriteLine($"Unknown role '{role}'. Valid roles: {string.Join(", ", RoleNames.All)}");
        Environment.ExitCode = 1;
        return true;
    }

    if (!await roleManager.RoleExistsAsync(role))
        await roleManager.CreateAsync(new IdentityRole(role));

    if (await userManager.FindByEmailAsync(newEmail) is not null)
    {
        Console.Error.WriteLine($"A user with email {newEmail} already exists.");
        Environment.ExitCode = 1;
        return true;
    }

    var newUser = new ApplicationUser
    {
        UserName = newEmail,
        Email = newEmail,
        FullName = fullName,
        EmailConfirmed = true,
        LockoutEnabled = true,
        IsActive = true
    };

    var createResult = await userManager.CreateAsync(newUser, password);
    if (!createResult.Succeeded)
    {
        foreach (var error in createResult.Errors)
            Console.Error.WriteLine(error.Description);
        Environment.ExitCode = 1;
        return true;
    }

    await userManager.AddToRoleAsync(newUser, role);
    Console.WriteLine($"Created user {newEmail} with role {role}. Sign in at /Account/Login.");
    return true;
}
