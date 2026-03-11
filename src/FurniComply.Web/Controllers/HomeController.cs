using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FurniComply.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace FurniComply.Web.Controllers;

public class HomeController : Controller
{
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
    public IActionResult RequestAccess()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
