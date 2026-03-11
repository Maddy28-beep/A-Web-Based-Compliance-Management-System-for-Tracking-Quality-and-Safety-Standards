using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FurniComply.Infrastructure.Identity;

namespace FurniComply.Web.Controllers;

[Authorize(Roles = RoleNames.SuperAdmin + "," + RoleNames.Admin)]
public class MasterDataController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
