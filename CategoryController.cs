using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaceDay_API.Database;
using RaceDay_API.Models;

namespace RaceDay_API.Controllers
{// start of namespace
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {// start of class

        // object of the helper class
        ManageCategory manage_category = new ManageCategory();

        // get request, any logged in user can view an event's categories
        [HttpGet("event/{eventId}")]
        public object get_categories(int eventId)
        {// start of method

            if (HttpContext.Session.GetInt32("UserID") == null)
            {
                return "please log in first";
            }
            return manage_category.get_categories_by_event(eventId);

        }// end of method

        // post request, organisers only
        [HttpPost]
        public string create_category(Category newCategory)
        {// start of method

            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return "please log in first";
            }
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return "only organisers can create categories";
            }
            return manage_category.add_category(newCategory, userId.Value);

        }// end of method

        // delete request, organisers only
        [HttpDelete("{id}")]
        public string delete_category(int id)
        {// start of method

            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return "please log in first";
            }
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return "only organisers can delete categories";
            }
            return manage_category.delete_category(id, userId.Value);

        }// end of method

    }// end of class
}// end of namespace