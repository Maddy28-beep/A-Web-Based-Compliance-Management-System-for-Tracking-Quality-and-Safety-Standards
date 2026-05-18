using System.Diagnostics;
using FurniComply.Domain.Entities;
using FurniComply.Infrastructure.Persistence;
using FurniComply.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FurniComply.Web.Models;

namespace FurniComply.Web.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;
    private readonly SupplierContactUniquenessService _contactUniqueness;

    public HomeController(AppDbContext db, SupplierContactUniquenessService contactUniqueness)
    {
        _db = db;
        _contactUniqueness = contactUniqueness;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult LoginInfo()
    {
        return Redirect("/Account/Login");
    }

    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null, bool retry = false)
    {
        // Legacy entry point: always delegate to Account/Login so there is a single login experience.
        return RedirectToAction("Login", "Account", new { returnUrl, retry });
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult RequestAccess()
    {
        return View(new RequestAccessViewModel());
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RequestAccess(RequestAccessViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (await _contactUniqueness.IsEmailInUseAsync(model.Email))
        {
            ModelState.AddModelError(
                nameof(model.Email),
                "This email is already registered. Sign in or use a different email address.");
            return View(model);
        }

        _db.AccessRequests.Add(new AccessRequest
        {
            Email = model.Email,
            FullName = model.FullName,
            Reason = model.Reason,
            PreferredRole = model.PreferredRole,
            Status = AccessRequestStatus.Pending
        });
        await _db.SaveChangesAsync();

        TempData["RequestAccessSuccess"] = true;
        return RedirectToAction(nameof(RequestAccess));
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
