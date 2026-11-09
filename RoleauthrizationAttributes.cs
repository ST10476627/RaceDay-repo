using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RaceDay_MVC.Filters
{// start of namespace
    public class RoleAuthorizeAttribute : ActionFilterAttribute
    {// start of class

        // the role allowed on the page, null means any logged in user
        private readonly string? role;

        public RoleAuthorizeAttribute(string? role = null)
        {
            this.role = role;
        }

        // runs before the page loads
        public override void OnActionExecuting(ActionExecutingContext context)
        {// start of method

            string? sessionRole = context.HttpContext.Session.GetString("Role");

            // not logged in, go to the login page
            if (sessionRole == null)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // logged in but the wrong role, send them to their own home page
            if (role != null && sessionRole != role)
            {
                if (sessionRole == "Organiser")
                {
                    context.Result = new RedirectToActionResult("Dashboard", "Organiser", null);
                }
                else
                {
                    context.Result = new RedirectToActionResult("Index", "Home", null);
                }
            }

        }// end of method

    }// end of class
}// end of namespace