using Microsoft.AspNetCore.Mvc;

namespace Minutes_Of_Meeting.Controllers
{
    public class MeetingsController : Controller
    {
        public IActionResult Index()
        {
            return View("Meeing_List");
        }
        public IActionResult Create()
        {
            return View("Create_Meeting");
        }
    }
}
