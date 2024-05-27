using System;
using System.Data.SqlClient;

namespace MyWebApplication2.Models
{
    public class UserTable
    {
        public static string con_string = "Server=tcp:ice1h-sql-sever.database.windows.net,1433;Initial Catalog=ice1h-sql-databse;Persist Security Info=False;User ID=Heath;Password=Jamie0406.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public int UserID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public int InsertUser(UserTable user)
        {
            try
            {
                string sql = "INSERT INTO Table_1(userName, userSurname, userEmail, userPassword) VALUES (@Name, @Surname, @Email, @Password)";
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@Name", user.Name);
                    cmd.Parameters.AddWithValue("@Surname", user.Surname);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
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

        public UserTable Login(string email, string password)
        {
            try
            {
                string sql = "SELECT * FROM Table_1 WHERE userEmail = @Email AND userPassword = @Password";
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        return new UserTable
                        {
                            UserID = Convert.ToInt32(rdr["userID"]),
                            Name = rdr["userName"].ToString(),
                            Surname = rdr["userSurname"].ToString(),
                            Email = rdr["userEmail"].ToString(),
                            Password = rdr["userPassword"].ToString()
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
