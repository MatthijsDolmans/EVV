using Microsoft.AspNetCore.Mvc;

namespace Evv.Controllers
{
    public class OverviewController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.page = "Overview";
            return View();
        }
    }
}
