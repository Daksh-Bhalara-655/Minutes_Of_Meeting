using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Minutes_Of_Meeting.DbConfig;
using Minutes_Of_Meeting.Models;
using Minutes_Of_Meeting.Services;
using System.Data;
using System.Data.Common;

namespace Minutes_Of_Meeting.Controllers
{
    public class StaffController : Controller
    {

        private readonly Db_Connection db_Connection;
        private readonly DepartmentList departmentList;

        public StaffController(Db_Connection db_Connection , DepartmentList departmentList)
        {
            this.db_Connection = db_Connection;
            this.departmentList = departmentList;
        }

        public IActionResult Index()
        {
            using (SqlConnection conn = db_Connection.CreateConnection())
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
                        MobileNo = reader["MobileNo"].ToString(),
                        EmailAddress = reader["EmailAddress"].ToString(),
                        Remarks = reader["Remarks"] == DBNull.Value ? null : reader["Remarks"].ToString(),
                        Created = Convert.ToDateTime(reader["Created"]),
                        Modified = Convert.ToDateTime(reader["Modified"])
                    });
                }

                ViewBag.DepartmentList = departmentList.GetDepartmentSelectList();

                return View("Staff_List", staffs);
            }
        }


        

public IActionResult Create(int? id)
        {
            Staff staff = new Staff();
            
            ViewBag.DepartmentList = departmentList.GetDepartmentSelectList();

            if (id != null && id > 0)
            {
                
                using (SqlConnection conn = db_Connection.CreateConnection())
                {
                    SqlCommand cmd = new SqlCommand("SP_GET_STAFF_BY_ID", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StaffID", id);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        staff.StaffID = Convert.ToInt32(reader["StaffID"]);
                        staff.StaffName = reader["StaffName"].ToString();
                        staff.DepartmentName = reader["DepartmentName"].ToString();
                        staff.MobileNo = reader["MobileNo"].ToString();
                        staff.EmailAddress = reader["EmailAddress"].ToString();
                        staff.Remarks = reader["Remarks"] == DBNull.Value ? null : reader["Remarks"].ToString();
                        staff.Created = Convert.ToDateTime(reader["Created"]);
                        staff.Modified = Convert.ToDateTime(reader["Modified"]);
                    }
                }
                
            }
           
            return View("Staff_Create" , staff);
        }

        public IActionResult Save(Staff model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DepartmentList = departmentList.GetDepartmentSelectList();
                return View("Staff_Create",model);
            }
            using (SqlConnection conn = db_Connection.CreateConnection())
                if(model.StaffID > 0)
                {
                    SqlCommand cmd = new SqlCommand("SP_UPDATE_STAFF", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StaffID", model.StaffID);
                    cmd.Parameters.AddWithValue("@StaffName", model.StaffName);
                    cmd.Parameters.AddWithValue("@DepartmentName", model.DepartmentName);
                    cmd.Parameters.AddWithValue("@MobileNo", model.MobileNo);
                    cmd.Parameters.AddWithValue("@EmailAddress", model.EmailAddress);
                    cmd.Parameters.AddWithValue("@Remarks", (object)model.Remarks ?? DBNull.Value);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine($"Updated StaffID: {model.StaffID}");
                }
                else
                {

                    SqlCommand cmd = new SqlCommand("SP_INSERT_STAFF", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StaffName", model.StaffName);
                    cmd.Parameters.AddWithValue("@DepartmentName", model.DepartmentName);
                    cmd.Parameters.AddWithValue("@EmailAddress", model.EmailAddress);
                    cmd.Parameters.AddWithValue("@MobileNo", model.MobileNo);
                    cmd.Parameters.AddWithValue("@Remarks", (object)model.Remarks ?? DBNull.Value);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine($"Inserted new staff: {model.StaffName}");
                }
           
            return RedirectToAction("Index");

        }

        public IActionResult Details(int id)
        {
            Staff staff = new Staff();
            using (SqlConnection conn = db_Connection.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_GET_STAFF_BY_ID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StaffID", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    staff.StaffID = Convert.ToInt32(reader["StaffID"]);
                    staff.StaffName = reader["StaffName"].ToString();
                    staff.DepartmentName = reader["DepartmentName"].ToString();
                    staff.MobileNo = reader["MobileNo"].ToString();
                    staff.EmailAddress = reader["EmailAddress"].ToString();
                    staff.Remarks = reader["Remarks"] == DBNull.Value ? null : reader["Remarks"].ToString();
                    staff.Created = Convert.ToDateTime(reader["Created"]);
                    staff.Modified = Convert.ToDateTime(reader["Modified"]);
                }
            }
            return View("Staff_Details", staff);
        }

        public IActionResult Delete(int id)
        {
            using (SqlConnection conn = db_Connection.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_DELETE_STAFF", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StaffID", id);
                conn.Open();
                cmd.ExecuteNonQuery();
                Console.WriteLine($"Deleted StaffID: {id}");
            }
            return RedirectToAction("Index");

        }
        }
}
