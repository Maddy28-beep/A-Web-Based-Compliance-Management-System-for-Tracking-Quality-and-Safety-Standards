using FurniComply.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurniComply.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "System.Admin")]
public class BackupAdminController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;

    public BackupAdminController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [HttpPost("reset-password")]
    public IActionResult ResetBackupPassword()
    {
        if (!_environment.IsDevelopment())
        {
            return NotFound();
        }

        return StatusCode(
            StatusCodes.Status410Gone,
            "Direct backup-admin password reset has been retired. Use the normal password-reset flow.");
    }
}
