using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Evv.Classes;
using Evv.Models;
using Evv.Database;
using System.Net.Mail;

namespace Evv.Controllers
{
    public class AccountController : Controller
    {
        [Route("Login")]
        public async Task Login(string returnUrl = "/")
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(returnUrl)
                .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }

        public IActionResult Index()
        {
            //Console.WriteLine(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            //Console.WriteLine(User.Identity.Name);
            ViewBag.page = "Account";
            return View();
        }

        [Authorize]
        public async Task Logout()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                // Indicate here where Auth0 should redirect the user after a logout.
                // Note that the resulting absolute Uri must be added to the
                // **Allowed Logout URLs** settings for the app.
                .WithRedirectUri(Url.Action("Index", "Home"))
                .Build();

            // Logout from Auth0
            await HttpContext.SignOutAsync(
                Auth0Constants.AuthenticationScheme,
                authenticationProperties
            );
            // Logout from the application
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );
            HttpContext.Session.Remove("UserId");

        }

        [Authorize]
        public IActionResult PersonalData(string UserId)
        {
            AccountViewModel accountViewModel = new AccountViewModel();
            accountViewModel.Id = UserId;

            return View(accountViewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult PersonalData(string id, string firstname, string lastname)
        {
            DatabaseClass databaseClass = new DatabaseClass();
            string userId = HttpContext.Session.GetString("UserId");
            
            if(databaseClass.UpdateUser(firstname, lastname, id) < 1)
            {
                databaseClass.CreateUser(firstname, lastname, id);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
