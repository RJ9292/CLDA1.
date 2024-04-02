using Microsoft.AspNetCore.Mvc;

namespace MyWebApplication2.Controllers
{
    public class MyWorkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
