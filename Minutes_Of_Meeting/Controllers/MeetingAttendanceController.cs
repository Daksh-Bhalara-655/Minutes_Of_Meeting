using Microsoft.AspNetCore.Mvc;

namespace Minutes_Of_Meeting.Controllers
{
    public class MeetingAttendanceController : Controller
    {
        public IActionResult Index()
        {
            return View("Meeting_Attendance");
        }

    }
}
