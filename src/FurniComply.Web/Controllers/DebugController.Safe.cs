using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurniComply.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "System.Admin")]
public class DebugController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;

    public DebugController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [HttpGet("check-backup-admin")]
    public IActionResult CheckBackupAdmin()
    {
        if (!_environment.IsDevelopment())
        {
            return NotFound();
        }

        return StatusCode(
            StatusCodes.Status410Gone,
            new { message = "Debug backup-admin operations have been retired." });
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        if (!_environment.IsDevelopment())
        {
            return NotFound();
        }

        return Ok(new
        {
            message = "Debug controller is restricted to authenticated development-only use.",
            timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("list-users")]
    public IActionResult ListUsers()
    {
        if (!_environment.IsDevelopment())
        {
            return NotFound();
        }

        return StatusCode(
            StatusCodes.Status410Gone,
            new { message = "User enumeration debug endpoint has been retired." });
    }
}
