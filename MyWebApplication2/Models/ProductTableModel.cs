using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MyWebApplication2.Models
{
    public class ProductTableModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductAvailability { get; set; }
        public string ProductCategory { get; set; }

        public static string ConString { get; } = "Server=tcp:ice1h-sql-sever.database.windows.net,1433;Initial Catalog=ice1h-sql-databse;Persist Security Info=False;User ID=Heath;Password=Jamie0406.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public int InsertProduct(ProductTableModel p)
        {
            try
            {
                string sql = "INSERT INTO ProductTables(ProductName, ProductPrice, ProductCategory, ProductAvailability) VALUES (@ProductName, @ProductPrice, @ProductCategory, @ProductAvailability)";
                using (SqlConnection con = new SqlConnection(ConString))
                {
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
                    cmd.Parameters.AddWithValue("@ProductPrice", p.ProductPrice);
                    cmd.Parameters.AddWithValue("@ProductCategory", p.ProductCategory);
                    cmd.Parameters.AddWithValue("@ProductAvailability", p.ProductAvailability);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ProductTableModel> GetAllProducts()
        {
            List<ProductTableModel> products = new List<ProductTableModel>();

            using (SqlConnection con = new SqlConnection(ConString))
            {
                string sql = "SELECT ProductID, ProductName, ProductPrice, ProductCategory, ProductAvailability FROM ProductTables";
                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ProductTableModel product = new ProductTableModel
                    {
                        ProductID = Convert.ToInt32(rdr["ProductID"]),
                        ProductName = rdr["ProductName"] == DBNull.Value ? null : rdr["ProductName"].ToString(),
                        ProductPrice = rdr["ProductPrice"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["ProductPrice"]),
                        ProductCategory = rdr["ProductCategory"] == DBNull.Value ? null : rdr["ProductCategory"].ToString(),
                        ProductAvailability = rdr["ProductAvailability"] == DBNull.Value ? null : rdr["ProductAvailability"].ToString()
                    };
                    products.Add(product);
                }
            }

            return products;
        }

        public static ProductTableModel GetProductByID(int productID)
        {
            using (var connection = new SqlConnection(ConString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT ProductID, ProductName, ProductPrice, ProductCategory, ProductAvailability FROM ProductTables WHERE ProductID = @ProductID", connection);
                command.Parameters.AddWithValue("@ProductID", productID);
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new ProductTableModel
                    {
                        ProductID = reader.GetInt32(0),
                        ProductName = reader.GetString(1),
                        ProductPrice = reader.GetDecimal(2),
                        ProductCategory = reader.GetString(3),
                        ProductAvailability = reader.GetString(4)
                    };
                }
            }
            return null;
        }
    }

}
