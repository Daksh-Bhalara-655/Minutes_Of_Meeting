using Microsoft.AspNetCore.Mvc;
using Minutes_Of_Meeting.Models;

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
        public IActionResult Save(MeetingTypeModel model)
        {

            if (!ModelState.IsValid)
            {
                return View("Create_Meeting_Type", model);
            }
            return View("Meeting_Type_List");
        }
    }
}
