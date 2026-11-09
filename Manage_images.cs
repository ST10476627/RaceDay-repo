using System.Data.SqlClient;

namespace RaceDay_API.Database
{// start of namespace
    public class Manage_Images
    {// start of class

        string connection_string = @"Data source=(localdb)\api_both;database=RaceDay;";

        // save the banner address, only on this organiser's event
        public bool set_event_banner(int eventId, int organiserId, string url)
        {
            using (SqlConnection connect = new SqlConnection(connection_string))
            {
                connect.Open();
                string query = "UPDATE [Event] SET BannerUrl = @Url WHERE EventID = @ID AND OrganiserID = @OrganiserID;";
                SqlCommand run_query = new SqlCommand(query, connect);
                run_query.Parameters.AddWithValue("@Url", url);
                run_query.Parameters.AddWithValue("@ID", eventId);
                run_query.Parameters.AddWithValue("@OrganiserID", organiserId);
                return run_query.ExecuteNonQuery() > 0;
            }
        }

        // save the profile picture address for the logged in user
        public bool set_profile_picture(int userId, string url)
        {
            using (SqlConnection connect = new SqlConnection(connection_string))
            {
                connect.Open();
                string query = "UPDATE [User] SET ProfilePictureUrl = @Url WHERE UserID = @ID;";
                SqlCommand run_query = new SqlCommand(query, connect);
                run_query.Parameters.AddWithValue("@Url", url);
                run_query.Parameters.AddWithValue("@ID", userId);
                return run_query.ExecuteNonQuery() > 0;
            }
        }

    }// end of class
}// end of namespace