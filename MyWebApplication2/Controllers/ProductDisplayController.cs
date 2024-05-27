using MyWebApplication2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace MyWebApplication2.Controllers
{
    public class ProductDisplayController : Controller
    {
        private readonly ILogger<ProductDisplayController> _logger;

        public ProductDisplayController(ILogger<ProductDisplayController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var products = ProductTableModel.GetAllProducts();
            return View(products);
        }

        [HttpPost]
        public IActionResult AddProduct(ProductTableModel product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = new ProductTableModel().InsertProduct(product);
                    if (result > 0)
                    {
                        _logger.LogInformation("Product added successfully.");
                    }
                    else
                    {
                        _logger.LogWarning("Product insertion failed.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding product.");
                }

                return RedirectToAction("Index", "Home");
            }
            return View("MyWork");
        }
    }
}
