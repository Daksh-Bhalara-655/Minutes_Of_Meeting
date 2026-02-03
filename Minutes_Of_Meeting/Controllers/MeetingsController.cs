using Microsoft.AspNetCore.Mvc;
using Minutes_Of_Meeting.Models;

namespace Minutes_Of_Meeting.Controllers
{
    public class MeetingsController : Controller
    {
        public IActionResult Index()
        {
            return View("Meeting_List");
        }
        public IActionResult Create()
        {
            return View("Create_Meeting");
        }
        public IActionResult Save(MeetingsModel model)
        {
            if (!ModelState.IsValid)
            {
                return View ("Create_Meeting");
            }
            return View("Meeting_List");
        }
    }
}
