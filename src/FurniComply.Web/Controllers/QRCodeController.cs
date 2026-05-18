using FurniComply.Infrastructure.Identity;
using FurniComply.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FurniComply.Web.Controllers;

[Authorize]
public class QRCodeController : Controller
{
    private readonly QRCodeHelper _qrCodeHelper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<QRCodeController> _logger;

    public QRCodeController(
        QRCodeHelper qrCodeHelper,
        UserManager<ApplicationUser> userManager,
        ILogger<QRCodeController> logger)
    {
        _qrCodeHelper = qrCodeHelper;
        _userManager = userManager;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GenerateLoginQR()
    {
        try
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User not found" });
            }

            var result = await _qrCodeHelper.GenerateLoginQRAsync(userId);
            
            if (!result.Success)
            {
                return Json(new { success = false, message = result.ErrorMessage });
            }

            // Convert QR code to base64 for display
            var qrBase64 = Convert.ToBase64String(result.QRCode!);
            
            return Json(new { 
                success = true, 
                qrCode = qrBase64,
                qrText = result.QRText,
                userEmail = result.UserEmail,
                instructions = result.Instructions
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating QR code");
            return Json(new { success = false, message = "Error generating QR code" });
        }
    }
}
