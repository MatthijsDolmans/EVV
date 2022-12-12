using Evv.Database;
using Evv.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Evv.Controllers
{
    public class OverviewController : Controller
    {
        public IActionResult Index(OverviewViewModel viewModel, string button)
        {
            ViewBag.page = "Overview";
            string userid = HttpContext.Session.GetString("UserId");
            DatabaseClass databaseClass = new DatabaseClass();
            viewModel.trips = databaseClass.GetTrips(userid);
            viewModel.totalscore = databaseClass.TotalScore(userid);
            if (!string.IsNullOrEmpty(button))
            {
                viewModel.button = button;
            }
            if(HttpContext.Session.GetInt32("count") == null)
            {
                HttpContext.Session.SetInt32("count", 0);
            }
            return View(viewModel);
        }

        //

    }
}
