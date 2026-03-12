using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Minutes_Of_Meeting.DbConfig;
using System.Data;

namespace Minutes_Of_Meeting.Services
{
    public class DepartmentList
    {
        public List<SelectListItem> DepartmentSelectList { get; set; }

        private readonly Db_Connection db_Connection;

        public DepartmentList(Db_Connection db_Connection)
        {
            this.db_Connection = db_Connection;
            DepartmentSelectList = new List<SelectListItem>();
        }

        public List<SelectListItem> GetDepartmentSelectList()
        {
            // Ensure the list is fresh each call
            DepartmentSelectList.Clear();

            using (SqlConnection conn = db_Connection.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_GET_ALL_DEPARTMENTS", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DepartmentSelectList.Add(new SelectListItem
                        {
                            Text = reader["DepartmentName"].ToString(),
                            Value = reader["DepartmentID"].ToString()
                        });
                    }
                }
            }
            return DepartmentSelectList;
        }
    }
}
