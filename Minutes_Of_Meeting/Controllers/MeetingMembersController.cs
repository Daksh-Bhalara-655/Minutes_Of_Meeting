using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Minutes_Of_Meeting.Models;
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
        
            return View("Create_Meeting_Member");
        }

        public IActionResult Save(MeetingMembers model)
        {
            if(!ModelState.IsValid)
            {
                return View("Create_Meeting_Member", model);
            }

            return View("Meeting_Member_List");
        }


    }
}
