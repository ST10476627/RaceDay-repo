using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaceDay_API.Database;
using RaceDay_API.Models;

namespace RaceDay_API.Controllers
{// start of namespace
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {// start of class

        // object of the helper class
        ManageProfile manage_profile = new ManageProfile();

        // get request, both roles view their own profile
        [HttpGet]
        public object get_profile()
        {// start of method

            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return "please log in first";
            }

            User found = manage_profile.get_profile(userId.Value);
            if (found == null)
            {
                return "user not found";
            }
            return found;

        }// end of method

        // put request, both roles update their own profile
        [HttpPut]
        public string update_profile(string firstName, string lastName, string email)
        {// start of method

            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return "please log in first";
            }
            return manage_profile.update_profile(userId.Value, firstName, lastName, email);

        }// end of method

    }// end of class
}// end of namespace