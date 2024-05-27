using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MyWebApplication2.Models
{
    public class CartItemModel
    {
        public int CartID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductCategory { get; set; }
        public string ProductAvailability { get; set; }

        private static string connectionString = "Server=tcp:ice1h-sql-sever.database.windows.net,1433;Initial Catalog=ice1h-sql-databse;Persist Security Info=False;User ID=Heath;Password=Jamie0406.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private readonly ILogger<CartItemModel> _logger;

        // Restore the logger in the constructor
        public CartItemModel(ILogger<CartItemModel> logger)
        {
            _logger = logger;
        }

        public int AddToCart(CartItemModel cartItem)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO CartItems (UserID, ProductID, Quantity, ProductName, ProductPrice, ProductCategory, ProductAvailability) VALUES (@UserID, @ProductID, @Quantity, @ProductName, @ProductPrice, @ProductCategory, @ProductAvailability)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UserID", cartItem.UserID);
                cmd.Parameters.AddWithValue("@ProductID", cartItem.ProductID);
                cmd.Parameters.AddWithValue("@Quantity", cartItem.Quantity);
                cmd.Parameters.AddWithValue("@ProductName", cartItem.ProductName);
                cmd.Parameters.AddWithValue("@ProductPrice", cartItem.ProductPrice);
                cmd.Parameters.AddWithValue("@ProductCategory", cartItem.ProductCategory);
                cmd.Parameters.AddWithValue("@ProductAvailability", cartItem.ProductAvailability);

                try
                {
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding to cart.");
                    throw new Exception("Error adding to cart: " + ex.Message);
                }
            }
        }

        public List<CartItemModel> GetUserCartItems(int userID)
        {
            List<CartItemModel> cartItems = new List<CartItemModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = @"
                SELECT c.CartID, c.UserID, c.ProductID, c.Quantity, p.ProductName, p.ProductPrice, p.ProductCategory, p.ProductAvailability
                FROM CartItems c
                INNER JOIN ProductTables p ON c.ProductID = p.ProductID
                WHERE c.UserID = @UserID";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UserID", userID);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cartItems.Add(new CartItemModel(_logger)
                        {
                            CartID = Convert.ToInt32(reader["CartID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            ProductName = reader["ProductName"].ToString(),
                            ProductPrice = Convert.ToDecimal(reader["ProductPrice"]),
                            ProductCategory = reader["ProductCategory"].ToString(),
                            ProductAvailability = reader["ProductAvailability"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving cart items.");
                    throw new Exception("Error retrieving cart items: " + ex.Message);
                }
            }

            return cartItems;
        }

        public int UpdateCartItem(int cartID, int quantity)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "UPDATE CartItems SET Quantity = @Quantity WHERE CartID = @CartID";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@CartID", cartID);

                try
                {
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating cart item.");
                    throw new Exception("Error updating cart item: " + ex.Message);
                }
            }
        }

        public void ClearUserCart(int userID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "DELETE FROM CartItems WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UserID", userID);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    _logger.LogInformation("Cleared cart for user ID {UserID}", userID);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error clearing user cart.");
                    throw new Exception("Error clearing user cart: " + ex.Message);
                }
            }
        }
    }
}
