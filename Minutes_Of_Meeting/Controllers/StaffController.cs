using Microsoft.AspNetCore.Mvc;
using Minutes_Of_Meeting.Models;

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

        public IActionResult Save(Staff model)
        {
            if (!ModelState.IsValid)
            {
                
                return View("Staff_Create");
            }
            return View("Staff_List");
        }

    }
}
