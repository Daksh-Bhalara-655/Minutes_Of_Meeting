using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Minutes_Of_Meeting.DbConfig;
using Minutes_Of_Meeting.Models;
using System.Data;

namespace Minutes_Of_Meeting.Controllers
{
    public class StaffController : Controller
    {

        private readonly Db_Connection db_Connection;

        public StaffController(Db_Connection db_Connection)
        {
            this.db_Connection = db_Connection;
        }

        public IActionResult Index()
        {
            using (SqlConnection conn = new SqlConnection(db_Connection.GetWorkingConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("SP_GET_ALL_STAFF", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                List<Staff> staffs = new List<Staff>();

                while (reader.Read())
                {
                    staffs.Add(new Staff
                    {
                        StaffID = Convert.ToInt32(reader["StaffID"]),
                        StaffName = reader["StaffName"].ToString(),
                        DepartmentName = reader["DepartmentName"].ToString(),
                        EmailAddress = reader["EmailAddress"].ToString(),
                        Remarks = reader["Remarks"] == DBNull.Value ? null : reader["Remarks"].ToString(),
                        Created = Convert.ToDateTime(reader["Created"]),
                        Modified = Convert.ToDateTime(reader["Modified"])
                    });
                }

                var vm = new StaffListViewModel
                {
                    Staffs = staffs,

                    DepartmentList = staffs
                        .Select(s => s.DepartmentName)
                        .Distinct()
                        .Select(dept => new SelectListItem
                        {
                            Text = dept,
                            Value = dept
                        })
                        .ToList()
                };

                return View("Staff_List", vm);
            }
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
