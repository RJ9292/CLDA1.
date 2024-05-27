using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApplication2.Models;

namespace MyWebApplication2.Controllers
{
    public class UserController : Controller
    {
        private readonly UserTable userTB = new UserTable();

        [HttpPost]
        public ActionResult Login(UserTable Users)
        {
            var user = userTB.Login(Users.Email, Users.Password);
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserID", user.UserID);
                HttpContext.Session.SetString("UserName", user.Name);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View("~/Views/User/Login.cshtml");
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View("~/Views/User/SignUp.cshtml");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View("~/Views/User/Login.cshtml", new UserTable());
        }

        [HttpPost]
        public ActionResult About(UserTable user)
        {
            if (ModelState.IsValid)
            {
                int result = userTB.InsertUser(user);
                if (result > 0)
                {
                    ViewBag.Message = "User registered successfully.";
                }
                else
                {
                    ViewBag.Message = "Error while registering user.";
                }
            }
            return View("~/Views/User/SignUp.cshtml");
        }
    }
}
