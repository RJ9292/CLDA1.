using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
namespace MyWebApplication2.Models
{
    public class Table_1 
    {

        public static string con_string = "Server=tcp:ice1h-sql-sever.database.windows.net,1433;Initial Catalog=ice1h-sql-databse;Persist Security Info=False;User ID=Heath;Password=Jamie0406.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

        public static SqlConnection con = new SqlConnection(con_string);

        
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public int insert_User(Table_1 a)
        {
            try
            {
                string sql = "INSERT INTO Table_1(userName,userSurname,userEmail) VALUES (@Name,@Surname,@Email)";
                SqlCommand cmb = new SqlCommand(sql, con);
                cmb.Parameters.AddWithValue("@Name", a.Name);
                cmb.Parameters.AddWithValue("@Surname", a.Surname);
                cmb.Parameters.AddWithValue("@Email", a.Email);
                con.Open();
                int rowsAffected = cmb.ExecuteNonQuery();
                con.Close();
                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}