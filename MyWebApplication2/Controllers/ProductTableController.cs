using MyWebApplication2.Models;
using Microsoft.AspNetCore.Mvc;

namespace MyWebApplication2.Controllers
{
    public class ProductTableController : Controller
    {
        public Models.ProductTableModel prodtbl = new Models.ProductTableModel();

        [HttpPost]
        public ActionResult MyWork(Models.ProductTableModel products)
        {
            var result2 = prodtbl.InsertProduct(products); // Corrected method name
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult MyWork()
        {
            return View(prodtbl);
        }
    }
}
