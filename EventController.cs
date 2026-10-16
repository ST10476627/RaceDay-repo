using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaceDay_API.Database;
using RaceDay_API.Models;

namespace RaceDay_API.Controllers
{// start of namespace
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {// start of class

        // object of the helper class
        public Manage_Event manage_event = new Manage_Event();

        /// <summary>Returns all events. Any logged in user can view events.</summary>
        /// <returns>A list of events, or a message asking the user to log in</returns>
        [HttpGet]
        public object get_events()
        {// start of method

            if (HttpContext.Session.GetInt32("UserID") == null)
            {
                return "please log in first";
            }
            return manage_event.get_all_events();

        }// end of method

        /// <summary>Returns one event by its ID. Any logged in user can view it.</summary>
        /// <param name="id">The event ID</param>
        /// <returns>The event, or a message if it is not found</returns>
        [HttpGet("{id}")]
        public object get_event(int id)
        {// start of method

            if (HttpContext.Session.GetInt32("UserID") == null)
            {
                return "please log in first";
            }

            Event? found = manage_event.get_event_by_id(id);
            if (found == null)
            {
                return "event not found";
            }
            return found;

        }// end of method

        /// <summary>Creates a new event owned by the logged in Organiser. Organisers only.</summary>
        /// <param name="newEvent">Name, description, date, distance, event type (Run, Walk or Cycle) and location</param>
        /// <returns>A message saying whether the event was created</returns>
        [HttpPost]
        public string create_event(Event newEvent)
        {// start of method

            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return "please log in first";
            }
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return "only organisers can create events";
            }
            return manage_event.add_event(newEvent, userId.Value);

        }// end of method

        /// <summary>Updates one of the Organiser's own events. Organisers only.</summary>
        /// <param name="id">The ID of the event to update</param>
        /// <param name="updated">The new event details</param>
        /// <returns>A message saying whether the event was updated</returns>
        [HttpPut("{id}")]
        public string update_event(int id, Event updated)
        {// start of method

            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return "please log in first";
            }
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return "only organisers can update events";
            }
            return manage_event.update_event(id, updated, userId.Value);

        }// end of method

        /// <summary>Deletes one of the Organiser's own events. Organisers only. Events with enrolments cannot be deleted.</summary>
        /// <param name="id">The ID of the event to delete</param>
        /// <returns>A message saying whether the event was deleted</returns>
        [HttpDelete("{id}")]
        public string delete_event(int id)
        {// start of method

            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return "please log in first";
            }
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return "only organisers can delete events";
            }
            return manage_event.delete_event(id, userId.Value);

        }// end of method

    }// end of class
}// end of namespace