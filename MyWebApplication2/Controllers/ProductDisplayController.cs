using MyWebApplication2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace MyWebApplication2.Controllers
{
    public class ProductDisplayController : Controller
    {
        private readonly ILogger<ProductDisplayController> _logger;

        // Constructor to initialise the logger
        public ProductDisplayController(ILogger<ProductDisplayController> logger)
        {
            _logger = logger;
        }

        // Action to display the list of all products
        [HttpGet]
        public IActionResult Index()
        {
            var products = ProductTableModel.GetAllProducts();
            // Render the Index view with the list of products
            return View(products);
        }

        // Action to add a new product
        [HttpPost]
        public IActionResult AddProduct(ProductTableModel product)
        {
            // Check if the model state is valid
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

                // Redirect to the Home Index action after adding the product
                return RedirectToAction("Index", "Home");
            }
            // If the model state is invalid, render the MyWork view again
            return View("MyWork");
        }
    }
}
