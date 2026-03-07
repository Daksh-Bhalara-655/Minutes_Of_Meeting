using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Minutes_Of_Meeting.DbConfig;
using System.Data;

namespace Minutes_Of_Meeting.Services
{
    public class MeetingTypeList
    {
        private readonly Db_Connection db_Connection;
        public List<SelectListItem> MeetingTypeSelectList { get; set; }

        public MeetingTypeList(Db_Connection db_Connection)
        {
            this.db_Connection = db_Connection;
            MeetingTypeSelectList = new List<SelectListItem>();
        }
        
        public List<SelectListItem> GetMeetingTypeSelectList()
        {
            using (SqlConnection conn = db_Connection.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_GET_ALL_MEETING_TYPES", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    MeetingTypeSelectList.Add(new SelectListItem
                    {
                        Text = reader["MeetingTypeName"].ToString(),
                        Value = reader["MeetingTypeName"].ToString()
                    });
                }
            }
            return MeetingTypeSelectList;
        }
    }
}
