using System.Data.SqlClient;
using RaceDay_API.Models;

namespace RaceDay_API.Database
{// start of namespace
    public class ManageProfile
    {// start of class

        // connection string to the database
        string connection_string = @"Data source=(localdb)\api_both;database=RaceDay;";

        // get the details of the logged in user, without the password hash
        public User get_profile(int userId)
        {// start of method
            using (SqlConnection connect = new SqlConnection(connection_string))
            {
                connect.Open();
                string query = "SELECT UserID, FirstName, LastName, Email, RoleID FROM [User] WHERE UserID = @UserID;";
                SqlCommand run_query = new SqlCommand(query, connect);
                run_query.Parameters.AddWithValue("@UserID", userId);
                SqlDataReader reader = run_query.ExecuteReader();

                if (reader.Read())
                {
                    return new User()
                    {
                        UserID = (int)reader["UserID"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        RoleID = (int)reader["RoleID"],
                        PasswordHash = ""
                    };
                }
                return null;
            }
        }// end of method

        // update the details of the logged in user
        public string update_profile(int userId, string firstName, string lastName, string email)
        {// start of method
            try
            {
                using (SqlConnection connect = new SqlConnection(connection_string))
                {
                    connect.Open();

                    // the email cannot belong to a different user
                    string check_query = "SELECT COUNT(*) FROM [User] WHERE Email = @Email AND UserID <> @UserID;";
                    SqlCommand check = new SqlCommand(check_query, connect);
                    check.Parameters.AddWithValue("@Email", email);
                    check.Parameters.AddWithValue("@UserID", userId);
                    if ((int)check.ExecuteScalar() > 0)
                    {
                        return "email already used by another user";
                    }

                    string query = @"UPDATE [User]
                                     SET FirstName = @FirstName, LastName = @LastName, Email = @Email
                                     WHERE UserID = @UserID;";
                    SqlCommand run_query = new SqlCommand(query, connect);
                    run_query.Parameters.AddWithValue("@FirstName", firstName);
                    run_query.Parameters.AddWithValue("@LastName", lastName);
                    run_query.Parameters.AddWithValue("@Email", email);
                    run_query.Parameters.AddWithValue("@UserID", userId);

                    int rows = run_query.ExecuteNonQuery();
                    return rows > 0 ? "profile updated" : "user not found";
                }
            }
            catch (Exception error)
            {
                return error.Message;
            }
        }// end of method

    }// end of class
}// end of namespace