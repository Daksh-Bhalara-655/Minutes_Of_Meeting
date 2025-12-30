using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Minutes_Of_Meeting.Controllers
{
    public class MeetingMembersController : Controller
    {
       

        // GET: /MeetingMembers
        public IActionResult Index()
        {
          

            return View("Meeting_Member_List");
        }

        // GET: /MeetingMembers/Create
        public IActionResult Create()
        {
            // sample select data - replace with DB calls
        
            return View("Create_Meeting_Member");
        }

        
    }
}
