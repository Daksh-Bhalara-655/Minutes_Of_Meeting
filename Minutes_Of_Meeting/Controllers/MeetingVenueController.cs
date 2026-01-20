using Microsoft.AspNetCore.Mvc;
using Minutes_Of_Meeting.Models;

namespace Minutes_Of_Meeting.Controllers
{
    public class MeetingVenueController : Controller
    {
        public IActionResult Index()
        {
            return View("Meeting_Venue_List");
        }

        public IActionResult Create()
        {
            return View("Create_Meeting_Venue");
        }

        public IActionResult Save(MeetingVenue model)
        {
            if (!ModelState.IsValid)
            {
                return View("Create_Meeting_Venue", model);
            }
            return View("Meeting_Venue_List");
        }
    }
}
