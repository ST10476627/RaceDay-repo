namespace RaceDay_MVC.Models
{// start of namespace
    public class EventDetailsViewModel
    {// start of class

        public EventViewModel Event { get; set; } = new EventViewModel();
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();

    }// end of class
}// end of namespace