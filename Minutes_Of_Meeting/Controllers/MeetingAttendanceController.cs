using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Minutes_Of_Meeting.DbConfig;
using Minutes_Of_Meeting.Models;
using System.Data;

namespace Minutes_Of_Meeting.Controllers
{
    public class MeetingAttendanceController : Controller
    {
        private readonly Db_Connection db_Connection;   

        public MeetingAttendanceController(Db_Connection db_Connection)
        {
            this.db_Connection = db_Connection;
        }   


        public IActionResult Index()
        {

                return View("Meeting_Attendance");
            

        }

    }
}
