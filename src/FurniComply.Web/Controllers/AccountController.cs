using System;
using System.Text;
using System.Threading.Tasks;
using FurniComply.Domain.Entities;
using FurniComply.Infrastructure.Persistence;
using FurniComply.Infrastructure.Identity;
using FurniComply.Web.Models;
using FurniComply.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.WebUtilities;

namespace FurniComply.Web.Controllers;

public class AccountController : Controller
{
    private const string AuthenticationCategory = "Authentication";
    private const string DashboardControllerName = "Dashboard";
    private const string DashboardActionName = "Index";
    private const string LoginViewPath = "~/Views/Account/Login.cshtml";
    private const string ForgotPasswordViewPath = "~/Views/Account/ForgotPassword.cshtml";
    private const string ResetPasswordViewPath = "~/Views/Account/ResetPassword.cshtml";

    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _db;
    private readonly IReCaptchaService _recaptcha;
    private readonly IPasswordResetEmailSender _passwordResetMail;
    private readonly IPasswordHistoryService _passwordHistoryService;
    private readonly IConfiguration _config;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        AppDbContext db,
        IReCaptchaService recaptcha,
        IPasswordResetEmailSender passwordResetMail,
        IPasswordHistoryService passwordHistoryService,
        IConfiguration config)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _db = db;
        _recaptcha = recaptcha;
        _passwordResetMail = passwordResetMail;
        _passwordHistoryService = passwordHistoryService;
        _config = config;
    }

    [AllowAnonymous]
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Login(string? returnUrl = null, [FromQuery] bool rateLimit = false)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction(DashboardActionName, DashboardControllerName);
        }

        if (rateLimit)
        {
            ViewBag.RateLimitMessage = "Too many login attempts from your IP. Please wait a minute and try again.";
        }

        return View(LoginViewPath, new LoginViewModel { ReturnUrl = returnUrl });
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("Login")]
    public async Task<IActionResult> Login(
        LoginViewModel model,
        [FromForm(Name = "g-recaptcha-response")] string? gRecaptchaResponse)
    {
        if (!ModelState.IsValid)
            return View(LoginViewPath, model);

        if (!await VerifyRecaptchaAsync(gRecaptchaResponse, model.Email, "Login Failed"))
            return View(LoginViewPath, model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            LogSecurityEvent(model.Email, (string?)null, "Login Failed", "User not found");
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(LoginViewPath, model);
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);
        if (result.Succeeded)
        {
            LogSecurityEvent(user.Email ?? model.Email, user.Id, "Login Succeeded", "Successful sign-in");
            if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction(DashboardActionName, DashboardControllerName);
        }

        if (result.RequiresTwoFactor)
        {
            return RedirectToAction(nameof(TwoFactorChallenge), new { returnUrl = model.ReturnUrl, rememberMe = model.RememberMe });
        }

        if (result.IsLockedOut)
        {
            LogSecurityEvent(user.Email ?? model.Email, user.Id, "Login Failed", "Account locked due to too many failed login attempts");
            ModelState.AddModelError(string.Empty, "This account has been locked out due to too many failed login attempts. Please try again in 5 minutes.");
        }
        else
        {
            LogSecurityEvent(model.Email, user.Id, "Login Failed", "Invalid password");
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        return View(LoginViewPath, model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        var email = User.Identity?.Name;
        var userId = _userManager.GetUserId(User);
        await _signInManager.SignOutAsync();
        LogSecurityEvent(email, userId, "Logout", "User signed out");
        return RedirectToAction("Login", "Account");
    }

    [AllowAnonymous]
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult ForgotPassword() =>
        View(ForgotPasswordViewPath, new ForgotPasswordViewModel());

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("Login")]
    public async Task<IActionResult> ForgotPassword(
        ForgotPasswordViewModel model,
        [FromForm(Name = "g-recaptcha-response")] string? gRecaptchaResponse)
    {
        if (!ModelState.IsValid)
            return View(ForgotPasswordViewPath, model);

        if (!await VerifyRecaptchaAsync(gRecaptchaResponse, model.Email, "Password Reset Requested"))
            return View(ForgotPasswordViewPath, model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var appBaseUri = GetConfiguredAppBaseUri();
            var resetPath = Url.Action(
                nameof(ResetPassword),
                "Account",
                new { email = user.Email, code = token })!;
            var resetLink = new Uri(appBaseUri, resetPath).ToString();

            await _passwordResetMail.TrySendResetLinkAsync(user.Email ?? model.Email, user.FullName, resetLink);
        }

        return RedirectToAction(nameof(ForgotPasswordConfirmation));
    }

    [AllowAnonymous]
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View("~/Views/Account/ForgotPasswordConfirmation.cshtml");
    }

    [AllowAnonymous]
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult ResetPassword(string? email, string? code)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code))
        {
            return RedirectToAction(nameof(ForgotPassword));
        }

        return View(ResetPasswordViewPath, new ResetPasswordViewModel { Email = email, Code = code });
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("Login")]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(ResetPasswordViewPath, model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Reset failed. Please request a new password reset link.");
            return View(ResetPasswordViewPath, model);
        }

        if (await _passwordHistoryService.IsPasswordReuseAsync(user, model.Password))
        {
            ModelState.AddModelError(string.Empty, "You cannot reuse your current or recent passwords. Choose a new password.");
            return View(ResetPasswordViewPath, model);
        }

        string token;
        try
        {
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "This reset link is invalid or has expired. Request a new one.");
            return View(ResetPasswordViewPath, model);
        }

        var previousPasswordHash = user.PasswordHash;
        var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(ResetPasswordViewPath, model);
        }

        await _passwordHistoryService.RememberPreviousPasswordAsync(user, previousPasswordHash);

        // Sign in so the auth cookie is established; avoids anonymous-only edge cases after PRG redirect.
        await _signInManager.SignInAsync(user, isPersistent: false);

        TempData["SuccessMessage"] = "Your password has been reset and you are signed in.";
        var dashboardUrl = Url.Action(DashboardActionName, DashboardControllerName);
        return string.IsNullOrEmpty(dashboardUrl)
            ? RedirectToAction(DashboardActionName, DashboardControllerName)
            : LocalRedirect(dashboardUrl);
    }

    [AllowAnonymous]
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult ResetPasswordConfirmation()
    {
        return View("~/Views/Account/ResetPasswordConfirmation.cshtml");
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult AccessDenied(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult TwoFactorChallenge(string? returnUrl = null, bool rememberMe = false)
    {
        var model = new TwoFactorChallengeViewModel { ReturnUrl = returnUrl, RememberMe = rememberMe };
        return View(model);
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("Login")]
    public async Task<IActionResult> TwoFactorChallenge(TwoFactorChallengeViewModel model)
    {
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            return RedirectToAction(nameof(Login));
        }

        var result = await _signInManager.TwoFactorSignInAsync("Authenticator", model.Code, model.RememberMe, model.RememberMachine);
        if (result.Succeeded)
        {
            LogSecurityEvent(user.Email ?? user.UserName ?? "unknown", user.Id, "Login Succeeded", "Successful sign-in (2FA)");
            if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction(DashboardActionName, DashboardControllerName);
        }

        if (result.IsLockedOut)
        {
            LogSecurityEvent(user.Email ?? user.UserName ?? "unknown", user.Id, "Login Failed", "Authenticator failures caused account lockout");
            return RedirectToAction(nameof(Login));
        }

        LogSecurityEvent(user.Email ?? user.UserName ?? "unknown", user.Id, "Login Failed", "Invalid authenticator code");
        ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Security()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction(nameof(Login));
        }

        if (user.TwoFactorEnabled)
        {
            return View("SecurityEnabled");
        }

        return RedirectToAction(nameof(EnableAuthenticator));
    }

    [HttpGet]
    public async Task<IActionResult> EnableAuthenticator()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction(nameof(Login));
        }

        if (user.TwoFactorEnabled)
        {
            TempData["InfoMessage"] = "Two-factor authentication is already enabled.";
            return RedirectToAction(DashboardActionName, DashboardControllerName);
        }

        var model = await BuildEnableAuthenticatorViewModelAsync(user);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction(nameof(Login));
        }

        if (!ModelState.IsValid)
        {
            await PopulateEnableAuthenticatorViewModelAsync(user, model);
            return View(model);
        }

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Authenticator", model.Code);
        if (!isValid)
        {
            ModelState.AddModelError(string.Empty, "Verification code is invalid.");
            await PopulateEnableAuthenticatorViewModelAsync(user, model);
            return View(model);
        }

        await _userManager.SetTwoFactorEnabledAsync(user, true);
        TempData["SuccessMessage"] = "Two-factor authentication has been enabled.";
        return RedirectToAction(DashboardActionName, DashboardControllerName);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DisableAuthenticator()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction(nameof(Login));
        }

        await _userManager.SetTwoFactorEnabledAsync(user, false);
        await _userManager.ResetAuthenticatorKeyAsync(user);
        TempData["SuccessMessage"] = "Two-factor authentication has been disabled.";
        return RedirectToAction(DashboardActionName, DashboardControllerName);
    }

    private static string FormatKey(string unformattedKey)
    {
        var result = new StringBuilder();
        for (int i = 0; i < unformattedKey.Length; i++)
        {
            if (i > 0 && i % 4 == 0) result.Append(' ');
            result.Append(unformattedKey[i]);
        }
        return result.ToString();
    }

    private static string GenerateQrCodeUri(string email, string unformattedKey)
    {
        return $"otpauth://totp/FurniComply:{email}?secret={unformattedKey}&issuer=FurniComply";
    }

    private Uri GetConfiguredAppBaseUri()
    {
        if (Request.Host.HasValue)
        {
            var requestOrigin = $"{Request.Scheme}://{Request.Host.Value}";
            if (Uri.TryCreate(requestOrigin, UriKind.Absolute, out var requestUri))
            {
                return requestUri;
            }
        }

        var configuredOrigin = _config["App:PublicOrigin"];
        if (Uri.TryCreate(configuredOrigin, UriKind.Absolute, out var configuredUri))
        {
            return configuredUri;
        }

        throw new InvalidOperationException("Password reset links require a valid absolute App:PublicOrigin configuration.");
    }

    [HttpGet]
    public async Task<IActionResult> AuthenticatorQrCode()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var key = await _userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(key))
        {
            return NotFound();
        }

        var uri = GenerateQrCodeUri(user.Email ?? user.UserName ?? "user", key);
        using var qr = new QRCoder.QRCodeGenerator();
        var qrData = qr.CreateQrCode(uri, QRCoder.QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new QRCoder.PngByteQRCode(qrData);
        var pngBytes = qrCode.GetGraphic(4);
        return File(pngBytes, "image/png");
    }

    private async Task<bool> VerifyRecaptchaAsync(string? gRecaptchaResponse, string? email, string logAction)
    {
        if (!_recaptcha.IsEnabled)
            return true;

        var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        if (await _recaptcha.VerifyAsync(gRecaptchaResponse, remoteIp))
            return true;

        LogSecurityEvent(email, null, logAction, "reCAPTCHA verification failed");
        ModelState.AddModelError(string.Empty, "Verification failed. Please complete the captcha and try again.");
        return false;
    }

    private void LogSecurityEvent(string? email, string? userId, string action, string details)
    {
        try
        {
            _db.SecurityLogs.Add(new SecurityLog
            {
                Category = AuthenticationCategory,
                Action = action,
                Actor = email ?? "unknown",
                UserId = userId ?? string.Empty,
                IpAddress = GetClientIpAddress(),
                TimestampUtc = DateTime.UtcNow,
                Details = details
            });
            _db.SaveChanges();
        }
        catch
        {
            // Do not block login flow if logging fails
        }
    }

    private string GetClientIpAddress()
    {
        return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }

    private async Task<EnableAuthenticatorViewModel> BuildEnableAuthenticatorViewModelAsync(ApplicationUser user)
    {
        var model = new EnableAuthenticatorViewModel();
        await PopulateEnableAuthenticatorViewModelAsync(user, model);
        return model;
    }

    private async Task PopulateEnableAuthenticatorViewModelAsync(ApplicationUser user, EnableAuthenticatorViewModel model)
    {
        var authenticatorKey = await GetOrCreateAuthenticatorKeyAsync(user);
        model.SharedKey = FormatKey(authenticatorKey);
        model.AuthenticatorUri = GenerateQrCodeUri(user.Email ?? user.UserName ?? "user", authenticatorKey);
    }

    private async Task<string> GetOrCreateAuthenticatorKeyAsync(ApplicationUser user)
    {
        var authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(authenticatorKey))
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
            authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user);
        }

        return authenticatorKey!;
    }
}
