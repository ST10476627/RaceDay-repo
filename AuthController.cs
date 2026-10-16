using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaceDay_API.Database;
using RaceDay_API.Models;

namespace RaceDay_API.Controllers
{// start of namespace
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {// start of class

        // objects of the helper classes
        public RegisterUser register_user = new RegisterUser();
        public LoginUser login_user = new LoginUser();

        /// <summary>Registers a new user as an Organiser or a Participant.</summary>
        /// <param name="firstName">The user's first name</param>
        /// <param name="lastName">The user's last name</param>
        /// <param name="email">Must not already be registered</param>
        /// <param name="password">Stored as a BCrypt hash, never in plain text</param>
        /// <param name="roleId">1 for Organiser, 2 for Participant</param>
        /// <returns>A message saying whether the user was registered</returns>
        [HttpPost("register")]
        public string register(string firstName, string lastName,
                               string email, string password, int roleId)
        {
            return register_user.add_user(firstName, lastName, email, password, roleId);
        }

        /// <summary>Logs a user in and starts a session that stores their ID and role.</summary>
        /// <param name="email">The registered email</param>
        /// <param name="password">The password entered at registration</param>
        /// <returns>A message with the user's role, or an error if the details are wrong</returns>
        [HttpPost("login")]
        public string login(string email, string password)
        {
            User? found = login_user.check_login(email, password);

            if (found == null)
            {
                return "invalid email or password";
            }

            // store who is logged in, and their role, in the session
            HttpContext.Session.SetInt32("UserID", found.UserID);
            HttpContext.Session.SetString("Role", found.Role.RoleName);

            return "logged in as " + found.Role.RoleName;
        }

        /// <summary>Logs the user out and clears the session.</summary>
        /// <returns>A confirmation message</returns>
        [HttpPost("logout")]
        public string logout()
        {
            HttpContext.Session.Clear();
            return "logged out";
        }

    }// end of class
}// end of namespace