using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Minutes_Of_Meeting.Models;
using System.Data;

namespace Minutes_Of_Meeting.Controllers
{
    public class DepartmentController : Controller
    {

        public IActionResult Index()
        {
            List<DepartmentModel> departments = new List<DepartmentModel>();

            SqlConnection conn = new SqlConnection("Server=LAPTOP-9V1759OU\\SQLEXPRESS;Database=Minutes_of_Meeting_Management;Trusted_Connection=True;TrustServerCertificate=True");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SP_GET_ALL_DEPARTMENTS";
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) {
                DepartmentModel department = new DepartmentModel();
                department.DepartmentID = Convert.ToInt32(reader["DepartmentID"]);
                department.DepartmentName = reader["DepartmentName"].ToString();
                department.Created = Convert.ToDateTime(reader["created"]);
                department.Modified = Convert.ToDateTime(reader["modified"]);

                departments.Add(department);

            }
            reader.Close();
            conn.Close();


            return View("Department_List", departments);
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
