using Microsoft.AspNetCore.Mvc;

namespace MyWebApplication2.Controllers
{
    public class MyWorkController : Controller
    {
        // Action to display the MyWork view
        public IActionResult Index()
        {
            
            return View("~/Views/MyWork/MyWork.cshtml");
        }
    }
}
