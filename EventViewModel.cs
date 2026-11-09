namespace RaceDay_MVC.Models
{// start of namespace
    public class EventViewModel
    {// start of class

        public int EventID { get; set; }
        public string EventName { get; set; } = "";
        public string EventDescription { get; set; } = "";
        public DateTime EventDate { get; set; }
        public double Distance { get; set; }
        public string EventType { get; set; } = "";
        public string Location { get; set; } = "";
        public string? BannerUrl { get; set; }

    }// end of class
}// end of namespace