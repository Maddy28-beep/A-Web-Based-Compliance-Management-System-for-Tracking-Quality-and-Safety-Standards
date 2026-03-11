using System;
using System.Linq;
using System.Threading.Tasks;
using FurniComply.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FurniComply.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (context.Database.IsSqlite())
        {
            await context.Database.EnsureCreatedAsync();
        }
        else
        {
            await context.Database.MigrateAsync();
        }

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in RoleNames.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        await EnsureUserAsync(userManager, "superadmin@furnicomply.local", RoleNames.SuperAdmin, "Solarte545390!", "Super Admin");
        await EnsureUserAsync(userManager, "admin@furnicomply.local", RoleNames.Admin, "Angga545390!", "Operational Admin");
        await EnsureUserAsync(userManager, "compliance@furnicomply.local", RoleNames.ComplianceManager, "Cordova545390!", "Compliance Manager");
        await EnsureUserAsync(userManager, "depthead@furnicomply.local", RoleNames.DepartmentHead, "Dorador545390!", "Department Head");
        await EnsureUserAsync(userManager, "auditor@furnicomply.local", RoleNames.Auditor, "FurniComply!2026", "External Auditor");
        await EnsureUserAsync(userManager, "procurement@furnicomply.local", RoleNames.Procurement, "Palinne545390!", "Procurement Officer");
    }

    private static async Task EnsureUserAsync(UserManager<ApplicationUser> userManager, string email, string role, string password, string fullName)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create seed user '{email}': {errors}");
            }
        }
        else if (string.IsNullOrWhiteSpace(user.FullName))
        {
            user.FullName = fullName;
            await userManager.UpdateAsync(user);
        }
        else
        {
            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await userManager.ResetPasswordAsync(user, resetToken, password);
            if (!resetResult.Succeeded)
            {
                var errors = string.Join(", ", resetResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to reset seed user password '{email}': {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            await userManager.AddToRoleAsync(user, role);
        }

        var currentRoles = await userManager.GetRolesAsync(user);
        foreach (var currentRole in currentRoles.Where(r => !string.Equals(r, role, StringComparison.OrdinalIgnoreCase)))
        {
            await userManager.RemoveFromRoleAsync(user, currentRole);
        }
    }
}
