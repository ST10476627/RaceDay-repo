using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaceDay_API.Database;

namespace RaceDay_API.Controllers
{// start of namespace
    [Route("api/[controller]")]
    [ApiController]
    public class EnrolmentController : ControllerBase
    {// start of class

        // object of the helper class
        public ManageEnrolment manage_enrolment = new ManageEnrolment();

        /// <summary>Enrols the logged in Participant in an event by selecting a category. Participants only.</summary>
        /// <param name="eventId">The event to enter</param>
        /// <param name="categoryId">A category that belongs to that event</param>
        /// <returns>A message saying whether the enrolment was recorded</returns>
        [HttpPost]
        public string enrol(int eventId, int categoryId)
        {// start of method

            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return "please log in first";
            }
            if (HttpContext.Session.GetString("Role") != "Participant")
            {
                return "only participants can enrol in events";
            }
            return manage_enrolment.add_enrolment(userId.Value, eventId, categoryId);

        }// end of method

        /// <summary>Returns all enrolments for one of the Organiser's events. Organisers only.</summary>
        /// <param name="eventId">The event ID</param>
        /// <returns>A list of enrolments</returns>
        [HttpGet("event/{eventId}")]
        public object get_enrolments(int eventId)
        {// start of method

            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return "please log in first";
            }
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return "only organisers can view enrolments";
            }
            return manage_enrolment.get_enrolments_by_event(eventId, userId.Value);

        }// end of method

    }// end of class
}// end of namespace