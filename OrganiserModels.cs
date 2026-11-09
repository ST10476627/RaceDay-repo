namespace RaceDay_MVC.Models
{// start of namespace

    // one event with its number of enrolments, for the dashboard and events list
    public class EventSummaryViewModel
    {
        public int EventID { get; set; }
        public string EventName { get; set; } = "";
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = "";
        public string EventType { get; set; } = "";
        public double Distance { get; set; }
        public string? BannerUrl { get; set; }
        public int TotalEnrolments { get; set; }
    }

    // one enrolment on an event, with the participant's name
    public class OrganiserEnrolmentViewModel
    {
        public int EnrolmentID { get; set; }
        public string ParticipantName { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public string Status { get; set; } = "";
        public DateTime EnrolmentDate { get; set; }
        public bool HasResult { get; set; }
    }

}// end of namespace