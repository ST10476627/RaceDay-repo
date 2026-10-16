using System.Data.SqlClient;
using RaceDay_API.Models;

namespace RaceDay_API.Database
{// start of namespace
    public class LoginUser
    {// start of class

        // connection string to the database
        string connection_string = @"Data source=(localdb)\api_both;database=RaceDay;";

        // returns the user (with role) if the login is correct, otherwise null
        public virtual User? check_login(string email, string password)
        {// start of method
            using (SqlConnection connect = new SqlConnection(connection_string))
            {// start of using
                connect.Open();

                string query = @"SELECT u.UserID, u.password_hash, r.RoleName
                                 FROM [User] u
                                 INNER JOIN [Role] r ON u.RoleID = r.RoleID
                                 WHERE u.Email = @Email;";
                SqlCommand run_query = new SqlCommand(query, connect);
                run_query.Parameters.AddWithValue("@Email", email);

                SqlDataReader found_data = run_query.ExecuteReader();
                if (found_data.Read())
                {
                    string stored_hash = found_data["password_hash"].ToString();

                    // compare the entered password with the stored hash
                    bool password_ok = false;
                    try
                    {
                        password_ok = BCrypt.Net.BCrypt.Verify(password, stored_hash);
                    }
                    catch (Exception)
                    {
                        // a broken hash in the table counts as a failed login
                        password_ok = false;
                    }

                    if (password_ok)
                    {
                        return new User()
                        {
                            UserID = (int)found_data["UserID"],
                            Role = new Role() { RoleName = found_data["RoleName"].ToString() }
                        };
                    }
                }
                return null;
            }// end of using
        }// end of method

    }// end of class
}// end of namespace