using RaceDay_API.Models;
using System.Data.SqlClient;
namespace RaceDay_API.Database
{
    public class ManageCategory
    {// start of class

        // connection string to the database
        string connection_string = @"Data source=(localdb)\api_both;database=RaceDay;";

        // add a category to an event, only if the event belongs to this organiser
        public string add_category(Category newCategory, int organiserId)
        {// start of method
            try
            {
                using (SqlConnection connect = new SqlConnection(connection_string))
                {
                    connect.Open();
                    string query = @"INSERT INTO Category (CategoryName, CategoryType, EventID)
                                     SELECT @Name, @Type, @EventID
                                     WHERE EXISTS (SELECT 1 FROM [Event]
                                                   WHERE EventID = @EventID AND OrganiserID = @OrganiserID);";
                    SqlCommand run_query = new SqlCommand(query, connect);
                    run_query.Parameters.AddWithValue("@Name", newCategory.CategoryName);
                    run_query.Parameters.AddWithValue("@Type", newCategory.CategoryType);
                    run_query.Parameters.AddWithValue("@EventID", newCategory.EventID);
                    run_query.Parameters.AddWithValue("@OrganiserID", organiserId);

                    int rows = run_query.ExecuteNonQuery();
                    return rows > 0 ? "category created" : "event not found, or it is not yours";
                }
            }
            catch (Exception error)
            {
                return error.Message;
            }
        }// end of method

        // get all categories for one event
        public List<Category> get_categories_by_event(int eventId)
        {// start of method
            List<Category> categories = new List<Category>();
            using (SqlConnection connect = new SqlConnection(connection_string))
            {
                connect.Open();
                string query = "SELECT * FROM Category WHERE EventID = @EventID;";
                SqlCommand run_query = new SqlCommand(query, connect);
                run_query.Parameters.AddWithValue("@EventID", eventId);
                SqlDataReader reader = run_query.ExecuteReader();

                while (reader.Read())
                {
                    categories.Add(new Category()
                    {
                        CategoryID = (int)reader["CategoryID"],
                        CategoryName = reader["CategoryName"].ToString(),
                        CategoryType = reader["CategoryType"].ToString(),
                        EventID = (int)reader["EventID"]
                    });
                }
            }
            return categories;
        }// end of method

        // delete a category, only if it is on one of this organiser's events
        public string delete_category(int id, int organiserId)
        {// start of method
            try
            {
                using (SqlConnection connect = new SqlConnection(connection_string))
                {
                    connect.Open();

                    // a category that people enrolled in cannot be deleted
                    string check_query = "SELECT COUNT(*) FROM Enrolment WHERE CategoryID = @ID;";
                    SqlCommand check = new SqlCommand(check_query, connect);
                    check.Parameters.AddWithValue("@ID", id);
                    if ((int)check.ExecuteScalar() > 0)
                    {
                        return "cannot delete, participants are enrolled in this category";
                    }

                    string query = @"DELETE FROM Category WHERE CategoryID = @ID
                                     AND EventID IN (SELECT EventID FROM [Event] WHERE OrganiserID = @OrganiserID);";
                    SqlCommand run_query = new SqlCommand(query, connect);
                    run_query.Parameters.AddWithValue("@ID", id);
                    run_query.Parameters.AddWithValue("@OrganiserID", organiserId);

                    int rows = run_query.ExecuteNonQuery();
                    return rows > 0 ? "category deleted" : "category not found, or it is not yours";
                }
            }
            catch (Exception error)
            {
                return error.Message;
            }
        }// end of method

    }// end of class
}// end of namespace
