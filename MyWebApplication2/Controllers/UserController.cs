using Microsoft.AspNetCore.Mvc;
using MyWebApplication2.Models;

namespace MyWebApplication2.Controllers
{
    public class UserController : Controller
    {

        public Table_1 userTB = new Table_1();

        [HttpPost]
        public ActionResult About(Table_1 Users)
        {
            var result = userTB.insert_User(Users);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]

        public ActionResult About()
        {
            return View(userTB);
        }
    }
}