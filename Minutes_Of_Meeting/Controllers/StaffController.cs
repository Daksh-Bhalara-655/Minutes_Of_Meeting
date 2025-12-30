using Microsoft.AspNetCore.Mvc;

namespace Minutes_Of_Meeting.Controllers
{
    public class StaffController : Controller
    {
        public IActionResult Index()
        {
            return View("Staff_List");
        }

        public IActionResult Create()
        {
            // add new staff 
            return View("Staff_Create");
        }

    }
}
