using RaceDay_API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace RaceDay__unitTest
{// start of namespace
    public class AuthTests
    {// start of class
        // success: the correct password matches its hash
        [Fact]
        public void test_correctPassword()
        {// start of method
            string hashed = BCrypt.Net.BCrypt.HashPassword("12345");
            Assert.True(BCrypt.Net.BCrypt.Verify("12345", hashed));
        }// end of method

        // failure: a wrong password does not match
        [Fact]
        public void test_wrongPassword()
        {// start of method
            string hashed = BCrypt.Net.BCrypt.HashPassword("12345");
            Assert.False(BCrypt.Net.BCrypt.Verify("wrong", hashed));
        }// end of method

        // the password is never stored as plain text
        [Fact]
        public void test_passwordIsHashed()
        {// start of method
            string hashed = BCrypt.Net.BCrypt.HashPassword("12345");
            Assert.NotEqual("12345", hashed);
        }// end of method

        // success: logout clears the session
        [Fact]
        public void test_logoutClearsSession()
        {// start of method
            AuthController controller = TestHelper.WithSession(new AuthController(), 1, "Organiser");

            string result = controller.logout();

            Assert.Equal("logged out", result);
            Assert.Null(controller.HttpContext.Session.GetInt32("UserID"));
        }// end of method

    }// end of class
}// end of namespace
    
