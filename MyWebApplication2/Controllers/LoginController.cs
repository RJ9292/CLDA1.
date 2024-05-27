using MyWebApplication2.Models;
using Microsoft.AspNetCore.Mvc;

namespace MyWebApplication2.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginModel login;

        // Constructor to initialise the LoginModel
        public LoginController()
        {
            login = new LoginModel();
        }

        // Action to handle user login
        [HttpPost]
        public ActionResult Login(string email, string name)
        {
            var loginModel = new LoginModel();
            int userID = loginModel.SelectUser(email, name);

            if (userID != -1)
            {
                // Store the user ID in the session if the login is successful
                HttpContext.Session.SetInt32("UserID", userID);

                // Redirect to the User login page
                return RedirectToAction("Login", "User");
            }
            else
            {
                // Display the login failed view if the login is unsuccessful
                return View("LoginFailed");
            }
        }
    }
}
