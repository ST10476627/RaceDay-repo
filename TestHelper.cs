using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceDay__unitTest
{// start of namespace
    public class TestHelper
    {// start of class

        // gives the controller a fake session with the chosen user and role
        // pass null for both to test a user who is not logged in
        public static T WithSession<T>(T controller, int? userId, string? role) where T : ControllerBase
        {// start of method

            Fakesession session = new Fakesession();
            if (userId != null)
            {
                session.SetInt32("UserID", userId.Value);
            }
            if (role != null)
            {
                session.SetString("Role", role);
            }

            DefaultHttpContext context = new DefaultHttpContext();
            context.Session = session;
            controller.ControllerContext = new ControllerContext { HttpContext = context };

            return controller;

        }// end of method

    }// end of class
}// end of namespace