using FurniComply.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using QRCoder;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

using FurniComply.Web;

namespace FurniComply.Web.Services;

public class QRCodeHelper
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<QRCodeHelper> _logger;

    public QRCodeHelper(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        ILogger<QRCodeHelper> logger)
    {
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<QRCodeResult> GenerateLoginQRAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new QRCodeResult { Success = false, ErrorMessage = "User not found" };
            }

            // Generate a simple login prefix with user info
            var loginPrefix = GenerateLoginPrefix(user);
            var qrText = $"FC-LOGIN-{loginPrefix}-{DateTime.UtcNow:yyyyMMddHHmmss}";
            
            // Generate QR code
            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.M);
            using var qrCode = new PngByteQRCode(qrCodeData);
            var qrBytes = qrCode.GetGraphic(10);

            return new QRCodeResult
            {
                Success = true,
                QRCode = qrBytes,
                QRText = qrText,
                UserEmail = user.Email ?? user.UserName ?? "Unknown",
                Instructions = "Save this QR code for future quick login. Scan it when you need fast access to your account."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate login QR code for user {UserId}", userId);
            return new QRCodeResult { Success = false, ErrorMessage = SafeErrorMessages.QrGenerationFailed };
        }
    }

    private string GenerateLoginPrefix(ApplicationUser user)
    {
        // Create a simple prefix based on user info
        var email = user.Email ?? user.UserName ?? "user";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(email + user.Id));
        var prefix = Convert.ToHexString(hash)[..8].ToLower();
        return prefix;
    }
}

public class QRCodeResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public byte[]? QRCode { get; set; }
    public string? QRText { get; set; }
    public string? UserEmail { get; set; }
    public string? Instructions { get; set; }
}
