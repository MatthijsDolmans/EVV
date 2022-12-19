using Evv.Classes;
using Evv.Database;
using Evv.Models;
using Microsoft.AspNetCore.Mvc;

namespace Evv.Controllers
{
    public class LeaderboardController : Controller
    {
        public IActionResult Index()
        {
            SetUser();

            ViewBag.page = "Leaderboard";

            DatabaseClass databaseclass = new DatabaseClass();
            LeaderboardViewModel view = new LeaderboardViewModel();
            
            List<Person> people = databaseclass.GetAllPeople();

            foreach (Person person in people)
			{
                person.points = Math.Round(person.points, 2);
                person.distance = Math.Round(person.distance, 2);
            }

            List<Person> orderedPeople = people.OrderBy(p => p.points).ThenByDescending(p => p.distance).ThenByDescending(p => p.tripAmount).ToList();
            if(orderedPeople.Count <= 3 && orderedPeople.Count != 0)
			{
                view.top3 = new List<Person>();
                for (int i = 0; i < orderedPeople.Count; i++)
                {
                    view.top3.Add(orderedPeople[i]);
                }

                orderedPeople.RemoveRange(0, orderedPeople.Count);
            }
			else
			{
                view.top3 = new List<Person>();
                for (int i = 0; i < 3; i++)
                {
                    view.top3.Add(orderedPeople[i]);
                }

                orderedPeople.RemoveRange(0, 3);
            }

            view.people = orderedPeople;

            return View(view);
        }
        private void SetUser()
        {
            string userId = HttpContext.Session.GetString("UserId");
            DatabaseClass databaseClass = new DatabaseClass();

            ViewBag.Name = databaseClass.GetUserName(userId);
        }
    }
}
