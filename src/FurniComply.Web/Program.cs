using FurniComply.Infrastructure;
using FurniComply.Infrastructure.Identity;
using FurniComply.Web.Filters;
using FurniComply.Web.Services;
using FurniComply.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<TransactionFeedbackFilter>();
builder.Services.AddScoped<SupplierPerformanceService>();
builder.Services.AddScoped<ISupplierPerformanceService>(sp => sp.GetRequiredService<SupplierPerformanceService>());
builder.Services.AddScoped<ISupplierComplianceScoreService, SupplierComplianceScoreService>();
builder.Services.AddScoped<IAssignmentNotificationService, AssignmentEmailService>();
builder.Services.AddScoped<IProcurementService, ProcurementService>();
builder.Services.AddScoped<ISupplierManagementService, SupplierManagementService>();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.AddService<TransactionFeedbackFilter>();
});
builder.Services.AddRazorPages();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHttpClient();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 12;
    })
    .AddEntityFrameworkStores<FurniComply.Infrastructure.Persistence.AppDbContext>()
    .AddDefaultTokenProviders();

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
    static bool IsSuperAdminDenied(AuthorizationHandlerContext context) =>
        !context.User.IsInRole(FurniComply.Infrastructure.Identity.RoleNames.SuperAdmin);

    options.AddPolicy("System.Admin", policy =>
        policy.RequireRole(FurniComply.Infrastructure.Identity.RoleNames.SuperAdmin));

    options.AddPolicy("Suppliers.Override", policy =>
        policy.RequireRole(FurniComply.Infrastructure.Identity.RoleNames.SuperAdmin));

    options.AddPolicy("Policies.Read", policy =>
        policy.RequireRole(
            FurniComply.Infrastructure.Identity.RoleNames.SuperAdmin,
            FurniComply.Infrastructure.Identity.RoleNames.Admin,
            FurniComply.Infrastructure.Identity.RoleNames.ComplianceManager,
            FurniComply.Infrastructure.Identity.RoleNames.DepartmentHead,
            FurniComply.Infrastructure.Identity.RoleNames.Auditor));

    options.AddPolicy("Policies.Write", policy =>
    {
        policy.RequireRole(
            FurniComply.Infrastructure.Identity.RoleNames.Admin);
        policy.RequireAssertion(IsSuperAdminDenied);
    });

    options.AddPolicy("Policies.Approve", policy =>
    {
        policy.RequireRole(FurniComply.Infrastructure.Identity.RoleNames.Admin);
        policy.RequireAssertion(IsSuperAdminDenied);
    });

    options.AddPolicy("Compliance.Read", policy =>
        policy.RequireRole(
            FurniComply.Infrastructure.Identity.RoleNames.Admin,
            FurniComply.Infrastructure.Identity.RoleNames.ComplianceManager,
            FurniComply.Infrastructure.Identity.RoleNames.DepartmentHead,
            FurniComply.Infrastructure.Identity.RoleNames.Auditor));

    options.AddPolicy("Compliance.Write", policy =>
    {
        policy.RequireRole(
            FurniComply.Infrastructure.Identity.RoleNames.Admin,
            FurniComply.Infrastructure.Identity.RoleNames.ComplianceManager,
            FurniComply.Infrastructure.Identity.RoleNames.DepartmentHead);
        policy.RequireAssertion(IsSuperAdminDenied);
    });

    options.AddPolicy("Compliance.Verify", policy =>
    {
        policy.RequireRole(
            FurniComply.Infrastructure.Identity.RoleNames.Admin,
            FurniComply.Infrastructure.Identity.RoleNames.ComplianceManager);
        policy.RequireAssertion(IsSuperAdminDenied);
    });

    options.AddPolicy("Reports.Read", policy =>
        policy.RequireRole(
            FurniComply.Infrastructure.Identity.RoleNames.SuperAdmin,
            FurniComply.Infrastructure.Identity.RoleNames.Admin,
            FurniComply.Infrastructure.Identity.RoleNames.ComplianceManager,
            FurniComply.Infrastructure.Identity.RoleNames.DepartmentHead,
            FurniComply.Infrastructure.Identity.RoleNames.Auditor));

    options.AddPolicy("Reports.Write", policy =>
    {
        policy.RequireRole(FurniComply.Infrastructure.Identity.RoleNames.ComplianceManager);
        policy.RequireAssertion(IsSuperAdminDenied);
    });

    options.AddPolicy("Reports.Approve", policy =>
    {
        policy.RequireRole(
            FurniComply.Infrastructure.Identity.RoleNames.Admin,
            FurniComply.Infrastructure.Identity.RoleNames.ComplianceManager);
        policy.RequireAssertion(IsSuperAdminDenied);
    });

    options.AddPolicy("Reports.ApproveOverride", policy =>
    {
        policy.RequireRole(FurniComply.Infrastructure.Identity.RoleNames.SuperAdmin);
    });

    options.AddPolicy("Reports.Submit", policy =>
    {
        policy.RequireRole(FurniComply.Infrastructure.Identity.RoleNames.ComplianceManager);
        policy.RequireAssertion(IsSuperAdminDenied);
    });

    options.AddPolicy("Reports.Export", policy =>
    {
        policy.RequireRole(
            FurniComply.Infrastructure.Identity.RoleNames.SuperAdmin,
            FurniComply.Infrastructure.Identity.RoleNames.Admin,
            FurniComply.Infrastructure.Identity.RoleNames.ComplianceManager);
    });

    options.AddPolicy("Procurement.Read", policy =>
        policy.RequireRole(
            FurniComply.Infrastructure.Identity.RoleNames.SuperAdmin,
            FurniComply.Infrastructure.Identity.RoleNames.Admin,
            FurniComply.Infrastructure.Identity.RoleNames.ComplianceManager,
            FurniComply.Infrastructure.Identity.RoleNames.Procurement));

    options.AddPolicy("Procurement.Write", policy =>
    {
        policy.RequireRole(FurniComply.Infrastructure.Identity.RoleNames.Procurement);
        policy.RequireAssertion(IsSuperAdminDenied);
    });

    options.AddPolicy("Procurement.Approve", policy =>
    {
        policy.RequireRole(FurniComply.Infrastructure.Identity.RoleNames.Admin);
        policy.RequireAssertion(IsSuperAdminDenied);
    });

    options.AddPolicy("Procurement.PlaceOrder", policy =>
    {
        policy.RequireRole(FurniComply.Infrastructure.Identity.RoleNames.ComplianceManager);
        policy.RequireAssertion(IsSuperAdminDenied);
    });

    options.AddPolicy("Suppliers.Approve", policy =>
    {
        policy.RequireRole(FurniComply.Infrastructure.Identity.RoleNames.Admin);
        policy.RequireAssertion(IsSuperAdminDenied);
    });

    options.AddPolicy("Audit.Read", policy =>
    {
        policy.RequireRole(
            FurniComply.Infrastructure.Identity.RoleNames.SuperAdmin,
            FurniComply.Infrastructure.Identity.RoleNames.Admin,
            FurniComply.Infrastructure.Identity.RoleNames.ComplianceManager,
            FurniComply.Infrastructure.Identity.RoleNames.DepartmentHead,
            FurniComply.Infrastructure.Identity.RoleNames.Auditor,
            FurniComply.Infrastructure.Identity.RoleNames.Procurement);
    });

    options.AddPolicy("Audit.Export", policy =>
    {
        policy.RequireRole(
            FurniComply.Infrastructure.Identity.RoleNames.SuperAdmin,
            FurniComply.Infrastructure.Identity.RoleNames.Admin,
            FurniComply.Infrastructure.Identity.RoleNames.ComplianceManager);
    });

    // Legacy alias used by existing admin-only screens.
    options.AddPolicy("Admin", policy =>
        policy.RequireRole(FurniComply.Infrastructure.Identity.RoleNames.SuperAdmin));
});

var app = builder.Build();

var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrWhiteSpace(defaultConnection) &&
    defaultConnection.TrimStart().StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
{
    var dataDir = Path.Combine(app.Environment.ContentRootPath, "App_Data");
    Directory.CreateDirectory(dataDir);
}

var seedOnStartup = string.Equals(
    builder.Configuration["SeedDataOnStartup"],
    "true",
    StringComparison.OrdinalIgnoreCase);
var seedLiveScenariosOnStartup = string.Equals(
    builder.Configuration["SeedLiveScenariosOnStartup"],
    "true",
    StringComparison.OrdinalIgnoreCase);

await IdentitySeeder.SeedAsync(app.Services);

if (seedOnStartup)
{
    await FurniComply.Infrastructure.Persistence.DemoDataSeeder.SeedAsync(
        app.Services,
        seedLiveScenariosOnStartup ? 60 : 25,
        seedLiveScenariosOnStartup);
}

// Configure the HTTP request pipeline.
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
    // Hosted environments often use unknown proxy IPs; trust forwarded headers from the edge proxy.
    KnownNetworks = { },
    KnownProxies = { }
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

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

app.UseStatusCodePages(async context =>
{
    var httpContext = context.HttpContext;
    var requestPath = httpContext.Request.Path;

    if (httpContext.Response.StatusCode == StatusCodes.Status400BadRequest &&
        (requestPath.StartsWithSegments("/Account/Login", StringComparison.OrdinalIgnoreCase) ||
         requestPath.StartsWithSegments("/Home/Login", StringComparison.OrdinalIgnoreCase)))
    {
        // Recover from stale anti-forgery/request cookies by forcing a clean login page load.
        foreach (var cookieKey in httpContext.Request.Cookies.Keys)
        {
            if (cookieKey.StartsWith(".AspNetCore.Antiforgery", StringComparison.OrdinalIgnoreCase))
            {
                httpContext.Response.Cookies.Delete(cookieKey);
            }
        }

        httpContext.Response.Redirect("/Account/Login?retry=1");
    }
});

app.Use(async (context, next) =>
{
    var path = context.Request.Path;
        if (path == "/")
        {
            context.Response.Redirect("/Home/Index");
            return;
        }

        var isAnonymousPath =
            path.StartsWithSegments("/Home/Index", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWithSegments("/Home/Login", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWithSegments("/Account/Login", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWithSegments("/Home/RequestAccess", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWithSegments("/Home/Privacy", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWithSegments("/css") ||
            path.StartsWithSegments("/js") ||
            path.StartsWithSegments("/images") ||
            path.StartsWithSegments("/lib");

    if (!context.User.Identity?.IsAuthenticated ?? true)
    {
        if (!isAnonymousPath)
        {
            if (path.StartsWithSegments("/Dashboard", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.Redirect("/Account/Login?blocked=dashboard&returnUrl=%2FDashboard");
                return;
            }

            context.Response.Redirect("/Account/Login");
            return;
        }
    }

    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
