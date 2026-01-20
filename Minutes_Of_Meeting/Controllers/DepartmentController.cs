using Microsoft.AspNetCore.Mvc;
using Minutes_Of_Meeting.Models;

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

        public IActionResult Save(DepartmentModel model)
        {
            if(!ModelState.IsValid)
            {
                return View("Department_Create", model);
            }
            return View("Department_List");
        }
    }
}
