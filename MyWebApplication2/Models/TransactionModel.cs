using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MyWebApplication2.Models
{
    public class TransactionModel
    {
        public int TransactionID { get; set; }
        public decimal UserID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public DateTime OrderDate { get; set; }

        public int InsertTransaction(TransactionModel transaction)
        {
            using (var connection = new SqlConnection(ProductTableModel.ConString))
            {
                connection.Open();
                var commandText = @"
                INSERT INTO Transactions (UserID, ProductID, Quantity, ProductName, ProductPrice, OrderDate)
                VALUES (@UserID, @ProductID, @Quantity, @ProductName, @ProductPrice, @OrderDate)";

                var command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@UserID", transaction.UserID);
                command.Parameters.AddWithValue("@ProductID", transaction.ProductID);
                command.Parameters.AddWithValue("@Quantity", transaction.Quantity);
                command.Parameters.AddWithValue("@ProductName", transaction.ProductName);
                command.Parameters.AddWithValue("@ProductPrice", transaction.ProductPrice);
                command.Parameters.AddWithValue("@OrderDate", transaction.OrderDate);

                return command.ExecuteNonQuery();
            }
        }

        public List<TransactionModel> GetUserTransactions(decimal userID)
        {
            var transactions = new List<TransactionModel>();
            using (var connection = new SqlConnection(ProductTableModel.ConString))
            {
                connection.Open();
                var commandText = "SELECT * FROM Transactions WHERE UserID = @UserID";
                var command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@UserID", userID);
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
            return transactions;
        }
    }
}
