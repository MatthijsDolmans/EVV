using Evv.Database;
using Evv.Models;
using Microsoft.AspNetCore.Mvc;

namespace Evv.Controllers
{
    public class OverviewController : Controller
    {
        public IActionResult Index(OverviewViewModel viewModel)
        {
            ViewBag.page = "Overview";
            string userid = HttpContext.Session.GetString("UserId");
            DatabaseClass databaseClass = new DatabaseClass();
            viewModel.trips =  databaseClass.GetTrips(userid);
            return View(viewModel);
        }
    }
}
