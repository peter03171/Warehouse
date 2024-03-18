using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
