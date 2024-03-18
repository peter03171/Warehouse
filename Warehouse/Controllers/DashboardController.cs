using Microsoft.AspNetCore.Mvc;
using Warehouse.Data;

namespace Warehouse.Controllers
{
    public class DashboardController : Controller
    {
        private readonly WarehouseContext _context;
        public DashboardController(WarehouseContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var totalExpenses = await _context.OrderDetails
                 .SumAsync(od => od.Quantity * od.UnitPrice);

            var ordersByMonth = await _context.Orders
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    MonthYear = $"{group.Key.Month}/{group.Key.Year}",
                    Count = group.Count()
                })
                // 按照年月份排序
                .OrderBy(result => result.Year)
                .ThenBy(result => result.Month)
                .ToDictionaryAsync(k => k.MonthYear, v => v.Count);

            var viewModel = new DashboardViewModel
            {
                TotalOrders = await _context.Orders.CountAsync(),
                TotalExpenses = totalExpenses,
                OrdersByMonth = ordersByMonth
            };
            ViewBag.OrdersByMonthLabels = ordersByMonth.Keys.ToList();
            ViewBag.OrdersByMonthData = ordersByMonth.Values.ToList();

            return View(viewModel);
        }





    }
}
