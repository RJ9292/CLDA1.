using MyWebApplication2.Models;
using Microsoft.AspNetCore.Mvc;

namespace MyWebApplication2.Controllers
{
    public class ProductTableController : Controller
    {
        public Models.ProductTableModel prodtbl = new Models.ProductTableModel();

        // Action to handle adding a new product and redirecting to the Home index
        [HttpPost]
        public ActionResult MyWork(Models.ProductTableModel products)
        {
            var result2 = prodtbl.InsertProduct(products); // Insert the product into the database
            return RedirectToAction("Index", "Home");
        }

        // Action to display the MyWork view with the product table model
        [HttpGet]
        public ActionResult MyWork()
        {
            return View(prodtbl);
        }
    }
}
