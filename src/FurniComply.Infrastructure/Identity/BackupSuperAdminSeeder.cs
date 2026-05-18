using FurniComply.Domain.Entities;
using FurniComply.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace FurniComply.Infrastructure.Identity;

public class BackupSuperAdminSeeder
{
    public static async Task SeedBackupSuperAdminAsync(
        IServiceProvider serviceProvider,
        ILogger logger)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        try
        {
            var configuredPassword = configuration["BackupSuperAdmin:Password"]
                ?? configuration["BACKUP_SUPERADMIN_PASSWORD"];

            // Ensure SuperAdmin role exists
            var superAdminRole = await roleManager.FindByNameAsync(RoleNames.SuperAdmin);
            if (superAdminRole == null)
            {
                superAdminRole = new IdentityRole(RoleNames.SuperAdmin);
                await roleManager.CreateAsync(superAdminRole);
                logger.LogInformation("Created SuperAdmin role");
            }

            // Check if backup SuperAdmin already exists
            var existingBackupAdmin = await userManager.Users
                .FirstOrDefaultAsync(u => u.Email == "backup.admin@furnicomply.local");

            if (existingBackupAdmin == null)
            {
                // Create backup SuperAdmin account
                var backupAdmin = new ApplicationUser
                {
                    UserName = "backup.admin@furnicomply.local",
                    Email = "backup.admin@furnicomply.local",
                    FullName = "Backup Administrator",
                    EmailConfirmed = true,
                    IsBackupAccount = true,
                    IsHidden = true,
                    IsSystemAccount = true,
                    CreatedAtUtc = DateTime.UtcNow
                };

                // Generate a secure random password
                var backupPassword = string.IsNullOrWhiteSpace(configuredPassword)
                    ? GenerateSecurePassword()
                    : configuredPassword;
                
                var result = await userManager.CreateAsync(backupAdmin, backupPassword);
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(backupAdmin, RoleNames.SuperAdmin);
                    
                    logger.LogWarning(
                        "BACKUP SUPERADMIN CREATED - Email: {Email}. A one-time password was generated but will not be logged.",
                        backupAdmin.Email);
                    logger.LogInformation(
                        "Backup SuperAdmin account created successfully. Set or rotate its password through a secured admin workflow.");
                    
                    await db.SaveChangesAsync();
                }
                else
                {
                    logger.LogError("Failed to create backup SuperAdmin: {Errors}", 
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                existingBackupAdmin.IsBackupAccount = true;
                existingBackupAdmin.IsHidden = true;
                existingBackupAdmin.IsSystemAccount = true;
                await userManager.UpdateAsync(existingBackupAdmin);

                if (!await userManager.IsInRoleAsync(existingBackupAdmin, RoleNames.SuperAdmin))
                {
                    await userManager.AddToRoleAsync(existingBackupAdmin, RoleNames.SuperAdmin);
                }

                if (!string.IsNullOrWhiteSpace(configuredPassword))
                {
                    var resetToken = await userManager.GeneratePasswordResetTokenAsync(existingBackupAdmin);
                    var resetResult = await userManager.ResetPasswordAsync(existingBackupAdmin, resetToken, configuredPassword);

                    if (!resetResult.Succeeded)
                    {
                        logger.LogError(
                            "Failed to apply configured password for backup SuperAdmin: {Errors}",
                            string.Join(", ", resetResult.Errors.Select(error => error.Description)));
                    }
                }

                logger.LogInformation("Backup SuperAdmin account already exists: {Email}", existingBackupAdmin.Email);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while seeding backup SuperAdmin");
        }
    }

    private static string GenerateSecurePassword()
    {
        const string uppercase = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        const string lowercase = "abcdefghjkmnpqrstuvwxyz";
        const string digits = "23456789";
        const string special = "!@#$%^&*";
        var password = new char[16];
        var randomBytes = new byte[password.Length];
        RandomNumberGenerator.Fill(randomBytes);

        // Ensure at least one of each type
        password[0] = uppercase[randomBytes[0] % uppercase.Length];
        password[1] = lowercase[randomBytes[1] % lowercase.Length];
        password[2] = digits[randomBytes[2] % digits.Length];
        password[3] = special[randomBytes[3] % special.Length];

        // Fill the rest with random characters from all sets
        var allChars = uppercase + lowercase + digits + special;
        for (int i = 4; i < password.Length; i++)
        {
            password[i] = allChars[randomBytes[i] % allChars.Length];
        }

        // Shuffle the password
        var shuffleBytes = new byte[password.Length];
        RandomNumberGenerator.Fill(shuffleBytes);
        for (int i = password.Length - 1; i > 0; i--)
        {
            int j = shuffleBytes[i] % (i + 1);
            (password[i], password[j]) = (password[j], password[i]);
        }

        return new string(password);
    }
}
