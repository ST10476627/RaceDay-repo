namespace RaceDay_MVC.Models
{// start of namespace

    // one row on the My Events page
    public class EnrolmentViewModel
    {
        public int EnrolmentID { get; set; }
        public int EventID { get; set; }
        public string EventName { get; set; } = "";
        public DateTime EventDate { get; set; }
        public string CategoryName { get; set; } = "";
        public string Status { get; set; } = "";
        public DateTime EnrolmentDate { get; set; }
        public bool HasResult { get; set; }
    }

    // one row on the My Results page
    public class ResultViewModel
    {
        public string EventName { get; set; } = "";
        public DateTime EventDate { get; set; }
        public string CategoryName { get; set; } = "";
        public TimeSpan FinishTime { get; set; }
        public int FinishPosition { get; set; }
        public int TotalFinishers { get; set; }
    }

    // the logged in user's own details
    public class ProfileViewModel
    {
        public int UserID { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? ProfilePictureUrl { get; set; }
    }

}// end of namespace