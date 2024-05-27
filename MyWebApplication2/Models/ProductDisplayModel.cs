using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MyWebApplication2.Models
{
    public class ProductDisplayModel
    {
        // Properties for the product
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductCategory { get; set; }
        public bool ProductAvailability { get; set; }

        // Default constructor
        public ProductDisplayModel() { }

        // Parameterised constructor
        public ProductDisplayModel(int id, string name, decimal price, string category, bool availability)
        {
            ProductID = id;
            ProductName = name;
            ProductPrice = price;
            ProductCategory = category;
            ProductAvailability = availability;
        }

        // Method to get all products from the database
        public static List<ProductDisplayModel> SelectProducts()
        {
            List<ProductDisplayModel> products = new List<ProductDisplayModel>();

            // Connection string for the database
            string con_string = "Server=tcp:ice1h-sql-sever.database.windows.net,1433;Initial Catalog=ice1h-sql-databse;Persist Security Info=False;User ID=Heath;Password=Jamie0406.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection con = new SqlConnection(con_string))
            {
                string sql = "SELECT ProductID, ProductName, ProductPrice, ProductCategory, ProductAvailability FROM productTable";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ProductDisplayModel product = new ProductDisplayModel();
                    product.ProductID = Convert.ToInt32(reader["ProductID"]);
                    product.ProductName = Convert.ToString(reader["ProductName"]);
                    product.ProductPrice = Convert.ToDecimal(reader["ProductPrice"]);
                    product.ProductCategory = Convert.ToString(reader["ProductCategory"]);
                    product.ProductAvailability = Convert.ToBoolean(reader["ProductAvailability"]);
                    products.Add(product);
                }
                reader.Close();
            }
            return products;
        }
    }
}
