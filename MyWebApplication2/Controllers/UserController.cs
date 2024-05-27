using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApplication2.Models;

namespace MyWebApplication2.Controllers
{
    public class UserController : Controller
    {
        private readonly UserTable userTB = new UserTable();

        // Action to handle user login
        [HttpPost]
        public ActionResult Login(UserTable Users)
        {
            var user = userTB.Login(Users.Email, Users.Password);
            if (user != null)
            {
                // Store the user ID and name in the session if the login is successful
                HttpContext.Session.SetInt32("UserID", user.UserID);
                HttpContext.Session.SetString("UserName", user.Name);

                // Redirect to the Home index action
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Display an error message if the login is unsuccessful
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View("~/Views/User/Login.cshtml");
            }
        }

        // Action to handle user logout
        [HttpPost]
        public IActionResult Logout()
        {
            // Clear the session and redirect to the Home index action
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // Action to display the sign-up page
        [HttpGet]
        public ActionResult SignUp()
        {
            return View("~/Views/User/SignUp.cshtml");
        }

        // Action to display the login page
        [HttpGet]
        public ActionResult Login()
        {
            return View("~/Views/User/Login.cshtml", new UserTable());
        }
    }
}
