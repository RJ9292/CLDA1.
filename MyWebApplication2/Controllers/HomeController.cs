using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApplication2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace MyWebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<HomeController> _logger;
        private readonly ILogger<CartItemModel> _cartItemLogger;

        // Constructor to initialise the dependencies
        public HomeController(IHttpContextAccessor httpContextAccessor, ILogger<HomeController> logger, ILogger<CartItemModel> cartItemLogger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _cartItemLogger = cartItemLogger;
        }

        // Action to display the home page with a list of products
        public IActionResult Index()
        {
            var userID = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");

            if (userID.HasValue)
            {
                var userName = _httpContextAccessor.HttpContext.Session.GetString("UserName");
                List<ProductTableModel> products = ProductTableModel.GetAllProducts();

                // Pass the user ID and name to the view via ViewBag
                ViewBag.UserID = userID.ToString();
                ViewBag.UserName = userName;

                // Render the Index view with the list of products
                return View(products);
            }
            else
            {
                // Redirect to login if the user is not logged in
                return RedirectToAction("Login", "User");
            }
        }

        // Action to place an order for a product
        [HttpPost]
        public IActionResult PlaceOrder(int productID)
        {
            try
            {
                var userID = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
                if (userID.HasValue)
                {
                    var product = ProductTableModel.GetProductByID(productID);
                    if (product != null)
                    {
                        var cartItem = new CartItemModel(_cartItemLogger)
                        {
                            UserID = userID.Value,
                            ProductID = productID,
                            Quantity = 1, 
                            ProductName = product.ProductName,
                            ProductPrice = product.ProductPrice,
                            ProductCategory = product.ProductCategory,
                            ProductAvailability = product.ProductAvailability
                        };
                        cartItem.AddToCart(cartItem);
                    }
                }
                // Redirect to the Index action after placing the order
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the error and display an error page
                _logger.LogError(ex, "Error placing order");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = ex.Message });
            }
        }

        // Method to create or get a user table for storing their cart items
        private void CreateOrGetUserTable(string userTableName)
        {
            using (var connection = new SqlConnection(ProductTableModel.ConString))
            {
                connection.Open();
                var commandText = $@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{userTableName}' AND xtype='U')
                CREATE TABLE {userTableName} (
                    ProductID INT PRIMARY KEY,
                    ProductName NVARCHAR(50),
                    ProductPrice DECIMAL(18, 0),
                    Quantity INT,
                    ProductCategory NVARCHAR(100),
                    ProductAvailability NVARCHAR(100)
                )";

                // Log the SQL command being executed
                Console.WriteLine($"Executing SQL: {commandText}");

                var command = new SqlCommand(commandText, connection);
                command.ExecuteNonQuery();
            }
        }

        // Method to add a product to the user's cart table
        private void AddToUserCart(string userTableName, int productID)
        {
            var product = ProductTableModel.GetProductByID(productID);
            if (product != null)
            {
                using (var connection = new SqlConnection(ProductTableModel.ConString))
                {
                    connection.Open();
                    var commandText = $@"
                    IF EXISTS (SELECT 1 FROM {userTableName} WHERE ProductID = @ProductID)
                    UPDATE {userTableName}
                    SET Quantity = Quantity + 1
                    WHERE ProductID = @ProductID
                    ELSE
                    INSERT INTO {userTableName} (ProductID, ProductName, ProductPrice, Quantity, ProductCategory, ProductAvailability)
                    VALUES (@ProductID, @ProductName, @ProductPrice, @Quantity, @ProductCategory, @ProductAvailability)";

                    // Log the SQL command being executed
                    Console.WriteLine($"Executing SQL: {commandText}");

                    var command = new SqlCommand(commandText, connection);
                    command.Parameters.AddWithValue("@ProductID", productID);
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@ProductPrice", product.ProductPrice);
                    command.Parameters.AddWithValue("@Quantity", 1); // Assuming the default quantity is 1
                    command.Parameters.AddWithValue("@ProductCategory", product.ProductCategory);
                    command.Parameters.AddWithValue("@ProductAvailability", product.ProductAvailability);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Action to handle errors and display the error page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(ErrorViewModel errorViewModel)
        {
            return View(errorViewModel);
        }

        // Action to display the login page
        public IActionResult Login()
        {
            return View("~/Views/User/Login.cshtml");
        }
    }
}
