using System.Data.SqlClient;

namespace RaceDay_API.Database
{// start of namespace
    public class RegisterUser
    {// start of class

        // connection string to the database
        string connection_string = @"Data source=(localdb)\api_both;database=RaceDay;";

        public virtual string add_user(string firstName, string lastName,
                                       string email, string password, int roleId)
        {// start of method
            try
            {// start of try
                using (SqlConnection connect = new SqlConnection(connection_string))
                {// start of using
                    connect.Open();

                    // check that the email is not already registered
                    string check_query = "SELECT COUNT(*) FROM [User] WHERE Email = @Email;";
                    SqlCommand check = new SqlCommand(check_query, connect);
                    check.Parameters.AddWithValue("@Email", email);

                    int exists = (int)check.ExecuteScalar();
                    if (exists > 0)
                    {
                        return "email already registered";
                    }

                    // hash the password before storing it
                    string hashed_password = BCrypt.Net.BCrypt.HashPassword(password);

                    string query = @"INSERT INTO [User] (FirstName, LastName, Email, password_hash, RoleID)
                                     VALUES (@FirstName, @LastName, @Email, @Hash, @RoleID);";
                    SqlCommand run_query = new SqlCommand(query, connect);
                    run_query.Parameters.AddWithValue("@FirstName", firstName);
                    run_query.Parameters.AddWithValue("@LastName", lastName);
                    run_query.Parameters.AddWithValue("@Email", email);
                    run_query.Parameters.AddWithValue("@Hash", hashed_password);
                    run_query.Parameters.AddWithValue("@RoleID", roleId);

                    int rows = run_query.ExecuteNonQuery();
                    return rows > 0 ? "user registered" : "failed to register";
                }// end of using
            }// end of try
            catch (Exception error)
            {
                return error.Message;
            }
        }// end of method

    }// end of class
}// end of namespace