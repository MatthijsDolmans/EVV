using Evv.Classes;
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
            SetUser();
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

    
        public IActionResult Edit(int TripId)
        {
            SetUser();
            DatabaseClass _db = new DatabaseClass();
            Trip trip = _db.GetTripByID(TripId);
            TripViewModel viewModel = new TripViewModel();

            HttpContext.Session.SetInt32("tripID", trip.Id);

            viewModel.TripId = trip.Id;
            viewModel.Distance = trip.Distance;
            viewModel.People= trip.People;
            viewModel.score = trip.Score;
            viewModel.DateCreated = trip.DateCreated;

            foreach (Vehicle_Modifier item in Enum.GetValues(typeof(Vehicle_Modifier)))
            {
                if (item.ToString() == trip.Vehicle)
                {
                    viewModel.Vehicle_Modifier = item;
                }
            }

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult Edit(TripViewModel viewModel)
        {
            DatabaseClass _db = new();
            Trip trip = new Trip((int)HttpContext.Session.GetInt32("tripID"), viewModel.Distance, viewModel.Vehicle_Modifier, viewModel.DateCreated, viewModel.People, viewModel.Vehicle_Modifier.ToString());
            _db.UpdateTrip(trip);
            return RedirectToAction("Index");
        }
        
        public IActionResult Delete(int DeleteId)
        {
            DatabaseClass _db = new DatabaseClass();
            _db.DeleteTrip(DeleteId);

            return RedirectToAction("Index");
        }

        private void SetUser()
        {
            string userId = HttpContext.Session.GetString("UserId");
            DatabaseClass databaseClass = new DatabaseClass();

            ViewBag.Name = databaseClass.GetUserName(userId);
        }
    }
}
