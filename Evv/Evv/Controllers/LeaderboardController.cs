using Microsoft.AspNetCore.Mvc;

namespace Evv.Controllers
{
    public class LeaderboardController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.page = "Leaderboard";
            return View();
        }
    }
}
