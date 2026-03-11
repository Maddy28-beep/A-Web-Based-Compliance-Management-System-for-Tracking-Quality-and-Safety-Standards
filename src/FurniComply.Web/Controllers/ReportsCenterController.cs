using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using FurniComply.Domain.Entities;

namespace FurniComply.Web.Controllers;

[Authorize(Policy = "Reports.Read")]
public class ReportsCenterController : Controller
{
    private readonly FurniComply.Infrastructure.Persistence.AppDbContext _db;

    public ReportsCenterController(FurniComply.Infrastructure.Persistence.AppDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> SupplierPerformance()
    {
        var suppliers = await _db.Suppliers
            .Include(s => s.Performances)
            .Where(s => !s.IsDeleted)
            .OrderByDescending(s => s.PerformanceScore)
            .ToListAsync();

        return View(suppliers);
    }
}
