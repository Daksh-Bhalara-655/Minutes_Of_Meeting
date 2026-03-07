using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Minutes_Of_Meeting.DbConfig;
using Minutes_Of_Meeting.Models;
using System.Data;

namespace Minutes_Of_Meeting.Controllers
{
    public class DepartmentController : Controller
    {
    private readonly Db_Connection Db_Connection ;

        public DepartmentController(Db_Connection Db_Connection)
        {
            this.Db_Connection = Db_Connection;
        }


        public IActionResult Index()
        {
            List<DepartmentModel> departments = new List<DepartmentModel>();

            using (SqlConnection conn = Db_Connection.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SP_GET_ALL_DEPARTMENTS";
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DepartmentModel department = new DepartmentModel();
                    department.DepartmentID = Convert.ToInt32(reader["DepartmentID"]);
                    department.DepartmentName = reader["DepartmentName"].ToString();
                    department.Created = Convert.ToDateTime(reader["created"]);
                    department.Modified = Convert.ToDateTime(reader["modified"]);
                    departments.Add(department);
                }
                reader.Close();
            }
                return View("Department_List", departments);
        }


        public IActionResult Create(int? id )
        {
            DepartmentModel departmentModel = new DepartmentModel();
            if(id != null &&  id > 0)
            {
                using (SqlConnection conn = Db_Connection.CreateConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SP_GET_DEPARTMENT_BY_ID";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DepartmentID", id);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        departmentModel.DepartmentID = Convert.ToInt32(reader["DepartmentID"]);
                        departmentModel.DepartmentName = reader["DepartmentName"].ToString();
                        departmentModel.Created = Convert.ToDateTime(reader["created"]);
                        departmentModel.Modified = Convert.ToDateTime(reader["modified"]);
                    }
                    reader.Close();
                }
            }

            return View("Department_Create" , departmentModel);
        }

        public IActionResult Save(DepartmentModel model)
        {
            if(!ModelState.IsValid)
            {
                return View("Department_Create", model);
            }
            using (SqlConnection conn = Db_Connection.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                if(model.DepartmentID > 0)
                {
                    cmd.CommandText = "SP_UPDATE_DEPARTMENT";
                    cmd.Parameters.AddWithValue("@DepartmentID", model.DepartmentID);
                    Console.WriteLine("Updating Department with ID: " + model.DepartmentID);
                }
                else
                {
                    cmd.CommandText = "SP_INSERT_DEPARTMENT";
                    Console.WriteLine("Inserting new Department");
                }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DepartmentName", model.DepartmentName);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            using (SqlConnection conn = Db_Connection.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SP_DELETE_DEPARTMENT";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DepartmentID", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            DepartmentModel departmentModel = new DepartmentModel();

            using (SqlConnection conn = Db_Connection.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SP_GET_DEPARTMENT_BY_ID";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DepartmentID", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    //staff.StaffName = reader["StaffName"].ToString();
                    departmentModel.DepartmentID = Convert.ToInt32(reader["DepartmentID"]);
                    departmentModel.DepartmentName = reader["DepartmentName"].ToString();
                    departmentModel.Created = Convert.ToDateTime(reader["created"]);
                    departmentModel.Modified = Convert.ToDateTime(reader["modified"]);
                }
                else
                    {
                    Console.WriteLine("No department found with ID: " + id);
                }
                

            }
            return View("Department_Details", departmentModel);
        }
    }
}
