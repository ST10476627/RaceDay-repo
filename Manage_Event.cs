using System.Data.SqlClient;
using RaceDay_API.Models;

namespace RaceDay_API.Database
{// start of namespace
    public class Manage_Event
    {// start of class

        // connection string to the database
        string connection_string = @"Data source=(localdb)\api_both;database=RaceDay;";

        // create a new event for the logged in organiser
        public virtual string add_event(Event newEvent, int organiserId)
        {// start of method
            try
            {
                using (SqlConnection connect = new SqlConnection(connection_string))
                {
                    connect.Open();
                    string query = @"INSERT INTO [Event] (EventName, Event_Discrip, EventDate, Distance, EventType, Location, OrganiserID)
                                     VALUES (@Name, @Descrip, @Date, @Distance, @Type, @Location, @OrganiserID);";
                    SqlCommand run_query = new SqlCommand(query, connect);
                    run_query.Parameters.AddWithValue("@Name", newEvent.EventName);
                    run_query.Parameters.AddWithValue("@Descrip", newEvent.EventDescription);
                    run_query.Parameters.AddWithValue("@Date", newEvent.EventDate);
                    run_query.Parameters.AddWithValue("@Distance", newEvent.Distance);
                    run_query.Parameters.AddWithValue("@Type", newEvent.EventType);
                    run_query.Parameters.AddWithValue("@Location", newEvent.Location);
                    run_query.Parameters.AddWithValue("@OrganiserID", organiserId);

                    int rows = run_query.ExecuteNonQuery();
                    return rows > 0 ? "event created" : "failed to create event";
                }
            }
            catch (Exception error)
            {
                return error.Message;
            }
        }// end of method

        // get all events
        public virtual List<Event> get_all_events()
        {// start of method
            List<Event> events = new List<Event>();
            using (SqlConnection connect = new SqlConnection(connection_string))
            {
                connect.Open();
                string query = "SELECT * FROM [Event];";
                SqlCommand run_query = new SqlCommand(query, connect);
                SqlDataReader reader = run_query.ExecuteReader();

                while (reader.Read())
                {
                    events.Add(read_event(reader));
                }
            }
            return events;
        }// end of method

        // get one event by id, null if not found
        public virtual Event? get_event_by_id(int id)
        {// start of method
            using (SqlConnection connect = new SqlConnection(connection_string))
            {
                connect.Open();
                string query = "SELECT * FROM [Event] WHERE EventID = @ID;";
                SqlCommand run_query = new SqlCommand(query, connect);
                run_query.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = run_query.ExecuteReader();

                if (reader.Read())
                {
                    return read_event(reader);
                }
                return null;
            }
        }// end of method

        // update an event, only if it belongs to this organiser
        public virtual string update_event(int id, Event updated, int organiserId)
        {// start of method
            try
            {
                using (SqlConnection connect = new SqlConnection(connection_string))
                {
                    connect.Open();
                    string query = @"UPDATE [Event]
                                     SET EventName = @Name, Event_Discrip = @Descrip, EventDate = @Date,
                                         Distance = @Distance, EventType = @Type, Location = @Location
                                     WHERE EventID = @ID AND OrganiserID = @OrganiserID;";
                    SqlCommand run_query = new SqlCommand(query, connect);
                    run_query.Parameters.AddWithValue("@ID", id);
                    run_query.Parameters.AddWithValue("@OrganiserID", organiserId);
                    run_query.Parameters.AddWithValue("@Name", updated.EventName);
                    run_query.Parameters.AddWithValue("@Descrip", updated.EventDescription);
                    run_query.Parameters.AddWithValue("@Date", updated.EventDate);
                    run_query.Parameters.AddWithValue("@Distance", updated.Distance);
                    run_query.Parameters.AddWithValue("@Type", updated.EventType);
                    run_query.Parameters.AddWithValue("@Location", updated.Location);

                    int rows = run_query.ExecuteNonQuery();
                    return rows > 0 ? "event updated" : "event not found, or it is not yours";
                }
            }
            catch (Exception error)
            {
                return error.Message;
            }
        }// end of method

        // delete an event, only if it belongs to this organiser
        public virtual string delete_event(int id, int organiserId)
        {// start of method
            try
            {
                using (SqlConnection connect = new SqlConnection(connection_string))
                {
                    connect.Open();

                    // an event with enrolments cannot be deleted
                    string check_query = "SELECT COUNT(*) FROM Enrolment WHERE EventID = @ID;";
                    SqlCommand check = new SqlCommand(check_query, connect);
                    check.Parameters.AddWithValue("@ID", id);
                    if ((int)check.ExecuteScalar() > 0)
                    {
                        return "cannot delete, this event already has enrolments";
                    }

                    // delete its categories first, then the event
                    string delete_categories = @"DELETE FROM Category WHERE EventID = @ID
                                                 AND EXISTS (SELECT 1 FROM [Event] WHERE EventID = @ID AND OrganiserID = @OrganiserID);";
                    SqlCommand cat_command = new SqlCommand(delete_categories, connect);
                    cat_command.Parameters.AddWithValue("@ID", id);
                    cat_command.Parameters.AddWithValue("@OrganiserID", organiserId);
                    cat_command.ExecuteNonQuery();

                    string query = "DELETE FROM [Event] WHERE EventID = @ID AND OrganiserID = @OrganiserID;";
                    SqlCommand run_query = new SqlCommand(query, connect);
                    run_query.Parameters.AddWithValue("@ID", id);
                    run_query.Parameters.AddWithValue("@OrganiserID", organiserId);

                    int rows = run_query.ExecuteNonQuery();
                    return rows > 0 ? "event deleted" : "event not found, or it is not yours";
                }
            }
            catch (Exception error)
            {
                return error.Message;
            }
        }// end of method

        // turns one row from the reader into an Event object
        private Event read_event(SqlDataReader reader)
        {
            return new Event()
            {
                EventID = (int)reader["EventID"],
                EventName = reader["EventName"].ToString(),
                EventDescription = reader["Event_Discrip"].ToString(),
                EventDate = (DateTime)reader["EventDate"],
                Distance = Convert.ToDouble(reader["Distance"]),
                EventType = reader["EventType"].ToString(),
                Location = reader["Location"].ToString(),
                OrganiserID = (int)reader["OrganiserID"]
            };
        }

    }// end of class
}// end of namespace