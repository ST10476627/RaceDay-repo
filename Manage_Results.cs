using System.Data.SqlClient;
using RaceDay_API.Models;
using Results = RaceDay_API.Models.Results;
namespace RaceDay_API.Database
{ 
    public class Manage_Results
    {// start of class

        // connection string to the database
        string connection_string = @"Data source=(localdb)\api_both;database=RaceDay;";

        // capture a result, only for an enrolment on one of this organiser's events
        public string add_result(int enrolmentId, TimeSpan finishTime, int finishPosition, int organiserId)
        {// start of method
            try
            {
                using (SqlConnection connect = new SqlConnection(connection_string))
                {
                    connect.Open();

                    // the enrolment must belong to an event owned by this organiser
                    string owner_query = @"SELECT COUNT(*) FROM Enrolment e
                                           INNER JOIN [Event] ev ON e.EventID = ev.EventID
                                           WHERE e.EnrolmentID = @EnrolmentID AND ev.OrganiserID = @OrganiserID;";
                    SqlCommand owner_check = new SqlCommand(owner_query, connect);
                    owner_check.Parameters.AddWithValue("@EnrolmentID", enrolmentId);
                    owner_check.Parameters.AddWithValue("@OrganiserID", organiserId);
                    if ((int)owner_check.ExecuteScalar() == 0)
                    {
                        return "enrolment not found, or it is not for one of your events";
                    }
                    // an enrolment can only have one result
                    string duplicate_query = "SELECT COUNT(*) FROM Results WHERE EnrolmentID = @EnrolmentID;";
                    SqlCommand duplicate_check = new SqlCommand(duplicate_query, connect);
                    duplicate_check.Parameters.AddWithValue("@EnrolmentID", enrolmentId);
                    if ((int)duplicate_check.ExecuteScalar() > 0)
                    {
                        return "this enrolment already has a result";
                    }

                    string query = @"INSERT INTO Results (EnrolmentID, FinishTime, FinishPosition)
                                     VALUES (@EnrolmentID, @FinishTime, @FinishPosition);";
                    SqlCommand run_query = new SqlCommand(query, connect);
                    run_query.Parameters.AddWithValue("@EnrolmentID", enrolmentId);
                    run_query.Parameters.AddWithValue("@FinishTime", finishTime);
                    run_query.Parameters.AddWithValue("@FinishPosition", finishPosition);

                    int rows = run_query.ExecuteNonQuery();
                    return rows > 0 ? "result captured" : "failed to capture result";
                }
            }
            catch (Exception error)
            {
                return error.Message;
            }
        }// end of method

        // get the results of one participant, only their own
        public List<Results> get_results_by_participant(int participantId)
        {// start of method
            List<Results> results = new List<Results>();
            using (SqlConnection connect = new SqlConnection(connection_string))
            {
                connect.Open();
                string query = @"SELECT r.* FROM Results r
                                 INNER JOIN Enrolment e ON r.EnrolmentID = e.EnrolmentID
                                 WHERE e.ParticipantID = @ParticipantID;";
                SqlCommand run_query = new SqlCommand(query, connect);
                run_query.Parameters.AddWithValue("@ParticipantID", participantId);
                SqlDataReader reader = run_query.ExecuteReader();

                while (reader.Read())
                {
                    results.Add(new Results()
                    {
                        ResultsID = (int)reader["ResultsID"],
                        EnrolmentID = (int)reader["EnrolmentID"],
                        FinishTime = (TimeSpan)reader["FinishTime"],
                        FinishPosition = (int)reader["FinishPosition"]
                    });
                }
            }
            return results;
        }// end of method

    }// end of class
}// end of namespace


