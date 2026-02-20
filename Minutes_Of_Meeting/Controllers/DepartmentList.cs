using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Minutes_Of_Meeting.DbConfig;

namespace Minutes_Of_Meeting.Controllers
{
    public class DepartmentList
    {
        public List<SelectListItem> DepartmentSelectList { get; set; }

           private readonly Db_Connection Db_Connection;
        public DepartmentList(Db_Connection db_Connection)
            {
            this.Db_Connection = db_Connection;
            DepartmentSelectList = new List<SelectListItem>();
        }

        public List<SelectListItem> GetDepartmentSelectList()
        {
            using (SqlConnection conn = new SqlConnection(Db_Connection.GetWorkingConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("SP_GET_ALL_DEPARTMENTS", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DepartmentSelectList.Add(new SelectListItem
                    {
                        Text = reader["DepartmentName"].ToString(),
                        Value = reader["DepartmentName"].ToString()
                    });
                }
            }
            return DepartmentSelectList;
        }
        
    }
}
