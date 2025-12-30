using Microsoft.AspNetCore.Mvc;

namespace Minutes_Of_Meeting.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View("Department_List");
        }

        public IActionResult Create()
        {
            return View("Department_Create");
        }
    }
}
