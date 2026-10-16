using System.Data.SqlClient;
using RaceDay_API.Models;

namespace RaceDay_API.Database
{// start of namespace
    public class ManageEnrolment
    {// start of class

        // connection string to the database
        string connection_string = @"Data source=(localdb)\api_both;database=RaceDay;";

        // enrol a participant in an event by choosing a category
        public virtual string add_enrolment(int participantId, int eventId, int categoryId)
        {// start of method
            try
            {
                using (SqlConnection connect = new SqlConnection(connection_string))
                {
                    connect.Open();

                    // the category must belong to the event
                    string category_query = "SELECT COUNT(*) FROM Category WHERE CategoryID = @CategoryID AND EventID = @EventID;";
                    SqlCommand category_check = new SqlCommand(category_query, connect);
                    category_check.Parameters.AddWithValue("@CategoryID", categoryId);
                    category_check.Parameters.AddWithValue("@EventID", eventId);
                    if ((int)category_check.ExecuteScalar() == 0)
                    {
                        return "category does not belong to this event";
                    }

                    // a participant can only enrol once per event
                    string duplicate_query = "SELECT COUNT(*) FROM Enrolment WHERE ParticipantID = @ParticipantID AND EventID = @EventID;";
                    SqlCommand duplicate_check = new SqlCommand(duplicate_query, connect);
                    duplicate_check.Parameters.AddWithValue("@ParticipantID", participantId);
                    duplicate_check.Parameters.AddWithValue("@EventID", eventId);
                    if ((int)duplicate_check.ExecuteScalar() > 0)
                    {
                        return "already enrolled in this event";
                    }

                    // status is left out so the table default is used
                    string query = @"INSERT INTO Enrolment (ParticipantID, EnrolmentDate, EventID, CategoryID)
                                     VALUES (@ParticipantID, GETDATE(), @EventID, @CategoryID);";
                    SqlCommand run_query = new SqlCommand(query, connect);
                    run_query.Parameters.AddWithValue("@ParticipantID", participantId);
                    run_query.Parameters.AddWithValue("@EventID", eventId);
                    run_query.Parameters.AddWithValue("@CategoryID", categoryId);

                    int rows = run_query.ExecuteNonQuery();
                    return rows > 0 ? "enrolled successfully" : "failed to enrol";
                }
            }
            catch (Exception error)
            {
                return error.Message;
            }
        }// end of method

        // get all enrolments for an event, only if it belongs to this organiser
        public virtual List<Enrolment> get_enrolments_by_event(int eventId, int organiserId)
        {// start of method
            List<Enrolment> enrolments = new List<Enrolment>();
            using (SqlConnection connect = new SqlConnection(connection_string))
            {
                connect.Open();
                string query = @"SELECT e.* FROM Enrolment e
                                 INNER JOIN [Event] ev ON e.EventID = ev.EventID
                                 WHERE e.EventID = @EventID AND ev.OrganiserID = @OrganiserID;";
                SqlCommand run_query = new SqlCommand(query, connect);
                run_query.Parameters.AddWithValue("@EventID", eventId);
                run_query.Parameters.AddWithValue("@OrganiserID", organiserId);
                SqlDataReader reader = run_query.ExecuteReader();

                while (reader.Read())
                {
                    enrolments.Add(new Enrolment()
                    {
                        EnrolmentID = (int)reader["EnrolmentID"],
                        ParticipantID = (int)reader["ParticipantID"],
                        EnrolmentDate = (DateTime)reader["EnrolmentDate"],
                        EventID = (int)reader["EventID"],
                        CategoryID = (int)reader["CategoryID"],
                        EnrolmentStatus = reader["Enrolment_status"].ToString()
                    });
                }
            }
            return enrolments;
        }// end of method

    }// end of class
}// end of namespace