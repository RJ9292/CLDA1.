using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace MyWebApplication2.Models
{
    public class LoginModel
    {
        // Connection string for the database
        public static string con_string = "Server=tcp:ice1h-sql-sever.database.windows.net,1433;Initial Catalog=ice1h-sql-databse;Persist Security Info=False;User ID=Heath;Password=Jamie0406.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        // Method to check if a user exists in the database
        public int SelectUser(string email, string name)
        {
            int userId = -1; // Default value if user is not found
            using (SqlConnection con = new SqlConnection(con_string))
            {
                string sql = "SELECT userID FROM userTable WHERE userEmail = @Email AND userName = @Name";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Name", name);
                try
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        userId = Convert.ToInt32(result); // Get the user ID if found
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it appropriately
                    // For now, rethrow the exception
                    throw ex;
                }
            }
            return userId; // Return the user ID or -1 if not found
        }
    }
}
