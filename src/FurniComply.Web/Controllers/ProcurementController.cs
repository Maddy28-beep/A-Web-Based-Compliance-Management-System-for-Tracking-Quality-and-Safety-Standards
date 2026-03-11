using System.Threading.Tasks;
using FurniComply.Domain.Enums;
using FurniComply.Infrastructure.Identity;
using FurniComply.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurniComply.Web.Controllers;

[Authorize(Policy = "Procurement.Read")]
public class ProcurementController : Controller
{
    private readonly AppDbContext _db;

    public ProcurementController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.SupplierCount = await _db.Suppliers.CountAsync();
        ViewBag.OrdersOpen = await _db.ProcurementOrders.CountAsync(o =>
            o.ProcurementStatus != null &&
            o.ProcurementStatus.Name != "Closed" &&
            o.ProcurementStatus.Name != "Cancelled");
        ViewBag.OnHoldSuppliers = await _db.Suppliers.CountAsync(s => s.Status == SupplierStatus.OnHold);
        ViewBag.PendingSupplierApprovals = await _db.Suppliers.CountAsync(s => s.ApprovalStatus == SupplierApprovalStatus.Pending);
        ViewBag.PendingOrders = await _db.ProcurementOrders.CountAsync(o => o.ProcurementStatus != null && o.ProcurementStatus.Name == "Draft");
        ViewBag.TotalOpenOrderValue = await _db.ProcurementOrders
            .Where(o => o.ProcurementStatus != null && o.ProcurementStatus.Name != "Closed" && o.ProcurementStatus.Name != "Cancelled")
            .SumAsync(o => (decimal?)o.TotalAmount) ?? 0m;
        return View();
    }
}
