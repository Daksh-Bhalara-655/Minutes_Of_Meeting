using Microsoft.AspNetCore.Mvc;

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
    }
}
