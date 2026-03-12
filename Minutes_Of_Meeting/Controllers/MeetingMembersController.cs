using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Minutes_Of_Meeting.DbConfig;
using Minutes_Of_Meeting.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Minutes_Of_Meeting.Controllers
{
    public class MeetingMembersController : Controller
    {
        private readonly Db_Connection db_Connection;

        public MeetingMembersController(Db_Connection db_Connection)
        {
            this.db_Connection = db_Connection;
        }


        [HttpGet]
        [HttpGet]
        public IActionResult Index(int MeetingId, int DepartmentId)
        {
            ViewBag.MeetingId = MeetingId;

            List<Staff> staffs = new List<Staff>();

            using (SqlConnection conn = db_Connection.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_GET_ALL_STAFF_BY_DEPARTMENT_ID", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // ✅ Pass DepartmentId to SP
                cmd.Parameters.AddWithValue("@DEPARTMENTID", DepartmentId);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Staff staff = new Staff
                    {
                        StaffID = Convert.ToInt32(reader["StaffID"]),
                        StaffName = reader["StaffName"].ToString(),
                        DepartmentName = reader["DepartmentName"].ToString()
                    };

                    staffs.Add(staff);
                }
            }

            return View("Meeting_Member_List", staffs);
        }

        // GET: /MeetingMembers/Create
        [HttpPost]
        public IActionResult Create(int MeetingId, List<int> SelectedStaff)
        {
            using (SqlConnection conn = db_Connection.CreateConnection())
            {
                conn.Open();

                DataTable staffTable = new DataTable();
                staffTable.Columns.Add("StaffID", typeof(int));

                foreach (var staffId in SelectedStaff)
                {
                    staffTable.Rows.Add(staffId);
                }

                SqlCommand cmd = new SqlCommand("SP_INSERT_MEETING_MEMBER", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MEETINGID", MeetingId);
                cmd.Parameters.AddWithValue("@IsPresent", 1);
                cmd.Parameters.AddWithValue("@REMARKS", DBNull.Value);

                SqlParameter param = cmd.Parameters.AddWithValue("@STAFFLIST", staffTable);
                param.SqlDbType = SqlDbType.Structured;
                param.TypeName = "StaffIdTableType";

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index", "Meetings");
        }

        //public IActionResult Save(MeetingMembers model)
        //{
        //    if(!ModelState.IsValid)
        //    {
        //        return View("Create_Meeting_Member", model);
        //    }

        //    return View("Meeting_Member_List");
        //}


    }
}
