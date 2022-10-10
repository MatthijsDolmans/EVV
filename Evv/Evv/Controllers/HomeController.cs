using Evv.Classes;
using Evv.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Evv.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier) != null)
            {
                SetSession();
            }
            ViewBag.page = "Home";
            return View();
        }

        public IActionResult Privacy(TripViewModel viewModel)
        {
            Trip trip = new Trip(viewModel.Distance, Vehicle_Modifier.Motor, viewModel.People);
            viewModel.Distance = trip.GetDistance();
            viewModel.score = trip.CalculateScore();
            ViewBag.page = "Privacy";
            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void SetSession()
        {
            HttpContext.Session.SetString("UserId", User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            Console.WriteLine(HttpContext.Session.GetString("UserId"));
        }
    }
}