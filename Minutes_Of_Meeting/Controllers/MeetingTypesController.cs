using Microsoft.AspNetCore.Mvc;

namespace Minutes_Of_Meeting.Controllers
{
    public class MeetingTypesController : Controller
    {
        public IActionResult Index()
        {
            return View("Meeting_Type_List");
        }
        public IActionResult Create()
        {
            return View("Create_Meeting_Type");
        }
    }
}
