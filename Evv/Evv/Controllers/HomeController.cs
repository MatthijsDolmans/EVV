﻿using Evv.Classes;
using Evv.Database;
using Evv.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;
using System.Security.Claims;
using static System.Formats.Asn1.AsnWriter;

namespace Evv.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index(TripViewModel viewModel, string? submit, string? favorite,string? submit2)
        {
            if(viewModel.Vehicle_Modifier == Vehicle_Modifier.Walk_Bike || viewModel.Vehicle_Modifier == Vehicle_Modifier.Public_transport || viewModel.Vehicle_Modifier == Vehicle_Modifier.Airplane)
            {
                if (ModelState["People"] != null) ModelState["People"].Errors.Clear();
            }

            if (!ModelState.IsValid)
            { 
                return View(viewModel);
            }

                bool HasData = true;

            if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier) != null)
            {
                SetSession();
            }

            if (HttpContext.Session.GetString("UserId") != null)
            {
                HasData = HasAllUserData();
            }

            ViewBag.page = "Home";
            DatabaseClass db = new DatabaseClass();
            if (favorite != null)
            {
                SetUser();
                db.AddFavorite(HttpContext.Session.GetString("UserId"), viewModel.FavoriteName);
            }
            else
            {
                SetUser();

                if (submit == "Add trip")
                {
                    Trip trip = new Trip(viewModel.Distance, viewModel.Vehicle_Modifier, viewModel.People, viewModel.DateCreated, HttpContext.Session.GetString("UserId"));
                    viewModel.Distance = trip.GetDistance();
                    viewModel.score = trip.CalculateScore();
                    viewModel.DateCreated = trip.GetDate();
                    ViewBag.page = "Home";
                }
                else if (submit == "Add to favorites")
                {
                    Trip trip = new Trip(viewModel.Distance, viewModel.Vehicle_Modifier, viewModel.People, viewModel.DateCreated, HttpContext.Session.GetString("UserId"),viewModel.ChosenfavoriteName);
                    viewModel.Distance = trip.GetDistance();
                    viewModel.score = trip.CalculateScore();
                    viewModel.DateCreated = trip.GetDate();
                    ViewBag.page = "Home";
                }
                if(submit2 != null)
                {
                    db.AddTripFromFavorites(HttpContext.Session.GetString("UserId"), submit2);
                }
            }
            viewModel.FavoriteNames = db.GetFavoriteName(HttpContext.Session.GetString("UserId"));
            if (!HasData)
            {
                string userid = HttpContext.Session.GetString("UserId");

                return RedirectToAction("PersonalData", "Account", new { UserId = userid});
            }
            else
            {
                viewModel.DateCreated = DateTime.Now;
                return View(viewModel);
            }
        }

       // public IActionResult Index()
      //  {
      //      return View();
     //   }

  

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

        [Authorize]
        private bool HasAllUserData()
        {
            string userId = HttpContext.Session.GetString("UserId");

            DatabaseClass databaseClass = new DatabaseClass();
            Account account = databaseClass.GetCurentUserData(userId);
            if(account.FirstName == null || account.FirstName == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void SetUser()
        {
            string userId = HttpContext.Session.GetString("UserId");
            DatabaseClass databaseClass = new DatabaseClass();

            ViewBag.Name = databaseClass.GetUserName(userId);
        }
    }
}