using RaceDay_API.Controllers;
using RaceDay_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceDay__unitTest
{// start of namespace
    public class AcessTests
    {// start of class

        //not logged in 

        [Fact]
        public void test_events_notLoggedIn()
        {
            EventController controller = TestHelper.WithSession(new EventController(), null, null);
            Assert.Equal("please log in first", controller.get_events());
        }

        [Fact]
        public void test_createEvent_notLoggedIn()
        {
            EventController controller = TestHelper.WithSession(new EventController(), null, null);
            Assert.Equal("please log in first", controller.create_event(new Event()));
        }

        [Fact]
        public void test_enrol_notLoggedIn()
        {
            EnrolmentController controller = TestHelper.WithSession(new EnrolmentController(), null, null);
            Assert.Equal("please log in first", controller.enrol(1, 1));
        }

        [Fact]
        public void test_profile_notLoggedIn()
        {
            ProfileController controller = TestHelper.WithSession(new ProfileController(), null, null);
            Assert.Equal("please log in first", controller.get_profile());
        }

        //a Participant tries organiser functions

        [Fact]
        public void test_createEvent_participantRejected()
        {
            EventController controller = TestHelper.WithSession(new EventController(), 2, "Participant");
            Assert.Equal("only organisers can create events", controller.create_event(new Event()));
        }

        [Fact]
        public void test_updateEvent_participantRejected()
        {
            EventController controller = TestHelper.WithSession(new EventController(), 2, "Participant");
            Assert.Equal("only organisers can update events", controller.update_event(1, new Event()));
        }

        [Fact]
        public void test_deleteEvent_participantRejected()
        {
            EventController controller = TestHelper.WithSession(new EventController(), 2, "Participant");
            Assert.Equal("only organisers can delete events", controller.delete_event(1));
        }

        [Fact]
        public void test_createCategory_participantRejected()
        {
            CategoryController controller = TestHelper.WithSession(new CategoryController(), 2, "Participant");
            Assert.Equal("only organisers can create categories", controller.create_category(new Category()));
        }

        [Fact]
        public void test_viewEnrolments_participantRejected()
        {
            EnrolmentController controller = TestHelper.WithSession(new EnrolmentController(), 2, "Participant");
            Assert.Equal("only organisers can view enrolments", controller.get_enrolments(1));
        }

        [Fact]
        public void test_captureResult_participantRejected()
        {
            ResultsController controller = TestHelper.WithSession(new ResultsController(), 2, "Participant");
            Assert.Equal("only organisers can capture results", controller.capture_result(1, TimeSpan.FromMinutes(45), 1));
        }

        //an Organiser tries participant functions

        [Fact]
        public void test_enrol_organiserRejected()
        {
            EnrolmentController controller = TestHelper.WithSession(new EnrolmentController(), 1, "Organiser");
            Assert.Equal("only participants can enrol in events", controller.enrol(1, 1));
        }

        [Fact]
        public void test_myResults_organiserRejected()
        {
            ResultsController controller = TestHelper.WithSession(new ResultsController(), 1, "Organiser");
            Assert.Equal("only participants can view their own results", controller.get_my_results());
        }

    }// end of class
}// end of namespace