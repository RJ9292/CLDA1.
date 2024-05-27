using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApplication2.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MyWebApplication2.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ILogger<TransactionController> _logger;

        // Constructor to initialize the logger
        public TransactionController(ILogger<TransactionController> logger)
        {
            _logger = logger;
        }

        // Action to display the transaction history for the logged-in user
        [HttpGet]
        public IActionResult TransactionHistory()
        {
            // Get the user ID from the session
            int? userID = HttpContext.Session.GetInt32("UserID");
            if (userID.HasValue)
            {
                var userNumericID = Convert.ToDecimal(userID.Value);
                var transactions = new List<TransactionModel>();

                // Retrieve the transaction history from the database
                using (var connection = new SqlConnection(ProductTableModel.ConString))
                {
                    connection.Open();
                    var commandText = "SELECT * FROM Transactions WHERE UserID = @UserID";
                    var command = new SqlCommand(commandText, connection);
                    command.Parameters.AddWithValue("@UserID", userNumericID);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        transactions.Add(new TransactionModel
                        {
                            TransactionID = (int)reader["TransactionID"],
                            UserID = (decimal)reader["UserID"],
                            ProductID = (int)reader["ProductID"],
                            Quantity = (int)reader["Quantity"],
                            ProductName = reader["ProductName"].ToString(),
                            ProductPrice = (decimal)reader["ProductPrice"],
                            OrderDate = (DateTime)reader["OrderDate"]
                        });
                    }
                }

                // Render the TransactionHistory view with the list of transactions
                return View("~/Views/Transaction/TransactionHistory.cshtml", transactions);
            }
            else
            {
                // Redirect to login if the user is not logged in
                return RedirectToAction("Login", "User");
            }
        }
    }
}
