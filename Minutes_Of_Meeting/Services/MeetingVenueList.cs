using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Minutes_Of_Meeting.DbConfig;

namespace Minutes_Of_Meeting.Services
{
    public class MeetingVenueList
    {
        private readonly Db_Connection db_Connection;
        
        public List<SelectListItem> MeetingVenueSelectList { get; set; }

        public MeetingVenueList(Db_Connection db_Connection)
        {
            this.db_Connection = db_Connection;
            MeetingVenueSelectList = new List<SelectListItem>();
        }
        
        public List<SelectListItem> GetMeetingVenueSelectList()
        {
            MeetingVenueSelectList.Clear();

            using (SqlConnection conn = db_Connection.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_GET_ALL_MEETING_VENUES", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    MeetingVenueSelectList.Add(new SelectListItem
                    {
                        Text = reader["MeetingVenueName"].ToString(),
                        Value = reader["MeetingVenueID"].ToString()
                    });
                }
            }

            return MeetingVenueSelectList;
        }
         
        }
}
