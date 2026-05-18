using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurniComply.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        foreach (var role in RoleNames.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var seedUsers = ResolveSeedUsers(configuration);

        foreach (var seedUser in seedUsers)
        {
            await EnsureUserAsync(userManager, seedUser);
        }
    }

    private static IReadOnlyList<SeedUser> ResolveSeedUsers(IConfiguration configuration)
    {
        var configuredUsers = new[]
        {
            CreateSeedUser(configuration, "SuperAdmin", "superadmin@furnicomply.local", RoleNames.SuperAdmin, "Super Administrator", true),
            CreateSeedUser(configuration, "Admin", "admin@furnicomply.local", RoleNames.Admin, "Operational Admin", true),
            CreateSeedUser(configuration, "ComplianceManager", "compliance@furnicomply.local", RoleNames.ComplianceManager, "Compliance Manager", true),
            CreateSeedUser(configuration, "Procurement", "procurement@furnicomply.local", RoleNames.Procurement, "Procurement Officer", true),
            CreateSeedUser(configuration, "DepartmentHead", "depthead@furnicomply.local", RoleNames.DepartmentHead, "Department Head", true)
        };

        return configuredUsers
            .Where(user => user is not null)
            .Cast<SeedUser>()
            .ToArray();
    }

    private static SeedUser? CreateSeedUser(
        IConfiguration configuration,
        string configKey,
        string email,
        string role,
        string fullName,
        bool isSystemAccount)
    {
        var password = configuration[$"SeedIdentityPasswords:{configKey}"];
        if (string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        return new SeedUser(email, role, fullName, password, isSystemAccount);
    }

    private static async Task EnsureUserAsync(UserManager<ApplicationUser> userManager, SeedUser seedUser)
    {
        var user = await userManager.FindByEmailAsync(seedUser.Email);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = seedUser.Email,
                Email = seedUser.Email,
                FullName = seedUser.FullName,
                EmailConfirmed = true,
                LockoutEnabled = true,
                IsSystemAccount = seedUser.IsSystemAccount
            };

            var result = await userManager.CreateAsync(user, seedUser.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create seed user '{seedUser.Email}': {errors}");
            }
        }
        else
        {
            if (string.IsNullOrWhiteSpace(user.FullName))
            {
                user.FullName = seedUser.FullName;
                await userManager.UpdateAsync(user);
            }

            await EnsureSeedPasswordAsync(userManager, user, seedUser.Password);
        }

        if (!await userManager.IsInRoleAsync(user, seedUser.Role))
        {
            await userManager.AddToRoleAsync(user, seedUser.Role);
        }

        var currentRoles = await userManager.GetRolesAsync(user);
        foreach (var currentRole in currentRoles.Where(r => !string.Equals(r, seedUser.Role, StringComparison.OrdinalIgnoreCase)))
        {
            await userManager.RemoveFromRoleAsync(user, currentRole);
        }
    }

    private static async Task EnsureSeedPasswordAsync(
        UserManager<ApplicationUser> userManager,
        ApplicationUser user,
        string password)
    {
        IdentityResult result;
        if (await userManager.HasPasswordAsync(user))
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            result = await userManager.ResetPasswordAsync(user, token, password);
        }
        else
        {
            result = await userManager.AddPasswordAsync(user, password);
        }

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to set seed password for '{user.Email}': {errors}");
        }
    }

    private sealed record SeedUser(string Email, string Role, string FullName, string Password, bool IsSystemAccount = false);
}
