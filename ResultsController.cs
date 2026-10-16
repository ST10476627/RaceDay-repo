using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaceDay_API.Database;

namespace RaceDay_API.Controllers
{// start of namespace
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {// start of class

        // object of the helper class
        Manage_Results manage_results = new Manage_Results();

        // post request, organisers only
        [HttpPost]
        public string capture_result(int enrolmentId, TimeSpan finishTime, int finishPosition)
        {// start of method

            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return "please log in first";
            }
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return "only organisers can capture results";
            }
            return manage_results.add_result(enrolmentId, finishTime, finishPosition, userId.Value);

        }// end of method

        // get request, participants only, they see their own results
        [HttpGet("mine")]
        public object get_my_results()
        {// start of method

            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return "please log in first";
            }
            if (HttpContext.Session.GetString("Role") != "Participant")
            {
                return "only participants can view their own results";
            }
            return manage_results.get_results_by_participant(userId.Value);

        }// end of method

    }// end of class
}// end of namespace