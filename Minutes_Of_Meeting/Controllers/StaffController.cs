using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Minutes_Of_Meeting.Models;
using System.Data;

namespace Minutes_Of_Meeting.Controllers
{
    public class StaffController : Controller
    {
        public IActionResult Index()
        {
            List<Staff> staffs = new List<Staff>();
            SqlConnection conn = new SqlConnection("Server=LAPTOP-9V1759OU\\SQLEXPRESS;Database=Minutes_of_Meeting_Management;Trusted_Connection=True;TrustServerCertificate=True");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SP_GET_ALL_STAFF";
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Staff staff = new Staff();
                staff.StaffID = Convert.ToInt32(reader["StaffID"]);
                staff.DepartmentName = reader["DepartmentName"].ToString();
                staff.StaffName = reader["StaffName"].ToString();
                staff.EmailAddress = reader["EmailAddress"].ToString();
                staff.Remarks = reader["Remarks"] == DBNull.Value ? null : reader["Remarks"].ToString();
                staff.Created = Convert.ToDateTime(reader["Created"]);
                staff.Modified = Convert.ToDateTime(reader["Modified"]);
                staffs.Add(staff);
            }
            reader.Close();
            conn.Close();
            var departments = staffs
        .Select(s => s.DepartmentName.ToString()) // Assuming you want to show the ID or Name
        .Distinct()
        .Select(dept => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Text = dept,
            Value = dept
        })
        .ToList();

            // Use a clear name for your ViewBag property
            ViewBag.DepartmentList = departments;

            return View("Staff_List",staffs);
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
