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

        public TransactionController(ILogger<TransactionController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult TransactionHistory()
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            if (userID.HasValue)
            {
                var userNumericID = Convert.ToDecimal(userID.Value);
                var transactions = new List<TransactionModel>();
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
                return View("~/Views/Transaction/TransactionHistory.cshtml", transactions);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
    }
}
