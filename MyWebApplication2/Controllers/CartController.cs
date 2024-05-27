using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApplication2.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MyWebApplication2.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly ILogger<CartItemModel> _cartItemLogger;

        // Constructor to initialise the logger dependencies
        public CartController(ILogger<CartController> logger, ILogger<CartItemModel> cartItemLogger)
        {
            _logger = logger;
            _cartItemLogger = cartItemLogger;
        }

        // Action to display the user's cart
        public IActionResult Index()
        {
            var userID = HttpContext.Session.GetInt32("UserID");
            if (userID.HasValue)
            {
                var cartModel = new CartItemModel(_cartItemLogger);
                var cartItems = cartModel.GetUserCartItems(userID.Value);

                // Render the IndexCart view with the user cart items
                return View("~/Views/Cart/IndexCart.cshtml", cartItems);
            }
            else
            {
                // Redirect to login if the user is not logged in
                return RedirectToAction("Login", "User");
            }
        }

        // Action to add a product to the cart
        [HttpPost]
        public IActionResult AddToCart(int productID, int quantity)
        {
            var userID = HttpContext.Session.GetInt32("UserID");
            if (userID.HasValue)
            {
                var product = ProductTableModel.GetProductByID(productID);
                if (product != null)
                {
                    var cartItem = new CartItemModel(_cartItemLogger)
                    {
                        UserID = userID.Value,
                        ProductID = productID,
                        Quantity = quantity,
                        ProductName = product.ProductName,
                        ProductPrice = product.ProductPrice,
                        ProductCategory = product.ProductCategory,
                        ProductAvailability = product.ProductAvailability
                    };

                    // Add the product to the cart
                    cartItem.AddToCart(cartItem);
                }
                // Redirect back to the cart index
                return RedirectToAction("Index");
            }
            else
            {
                // Redirect to login if the user is not logged in
                return RedirectToAction("Login", "User");
            }
        }

        // Action to update the quantity of an item in the cart
        [HttpPost]
        public IActionResult UpdateCartItem(int cartID, int quantity)
        {
            var cartModel = new CartItemModel(_cartItemLogger);
            cartModel.UpdateCartItem(cartID, quantity);

            // Redirect back to the cart index
            return RedirectToAction("Index");
        }

        // Action to clear all items from the user cart
        [HttpPost]
        public IActionResult ClearCart()
        {
            var userID = HttpContext.Session.GetInt32("UserID");
            if (userID.HasValue)
            {
                var cartModel = new CartItemModel(_cartItemLogger);
                cartModel.ClearUserCart(userID.Value);
            }

            // Redirect back to the cart index
            return RedirectToAction("Index");
        }

        // Action to proceed with the order and create a transaction
        [HttpPost]
        public IActionResult ProceedWithOrder()
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            if (userID.HasValue)
            {
                var userNumericID = Convert.ToDecimal(userID.Value);
                var cartModel = new CartItemModel(_cartItemLogger);
                var cartItems = cartModel.GetUserCartItems(userID.Value);

                // Go through through each item in the cart and insert it into the Transactions table
                foreach (var item in cartItems)
                {
                    using (var connection = new SqlConnection(ProductTableModel.ConString))
                    {
                        connection.Open();
                        var commandText = @"
                        INSERT INTO Transactions (UserID, ProductID, Quantity, ProductName, ProductPrice, OrderDate)
                        VALUES (@UserID, @ProductID, @Quantity, @ProductName, @ProductPrice, @OrderDate)";

                        var command = new SqlCommand(commandText, connection);
                        command.Parameters.AddWithValue("@UserID", userNumericID);
                        command.Parameters.AddWithValue("@ProductID", item.ProductID);
                        command.Parameters.AddWithValue("@Quantity", item.Quantity);
                        command.Parameters.AddWithValue("@ProductName", item.ProductName);
                        command.Parameters.AddWithValue("@ProductPrice", item.ProductPrice);
                        command.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                        command.ExecuteNonQuery();
                    }
                }

                // Clear the user cart after the order is placed
                cartModel.ClearUserCart(userID.Value);

                // Redirect to the transaction history page
                return RedirectToAction("TransactionHistory", "Transaction");
            }
            else
            {
                // Redirect to login if the user is not logged in
                return RedirectToAction("Login", "User");
            }
        }
    }
}
