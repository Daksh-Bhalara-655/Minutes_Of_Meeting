using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Minutes_Of_Meeting.DbConfig;
using Minutes_Of_Meeting.Models;
using Minutes_Of_Meeting.Services;
using System.Data;

namespace Minutes_Of_Meeting.Controllers
{
    public class MeetingsController : Controller
    {
        private readonly Db_Connection dbConnection;
        private readonly DepartmentList departmentList;
        private readonly MeetingTypeList meetingTypeList;

        public MeetingsController(Db_Connection dbConnection, DepartmentList departmentList , MeetingTypeList meetingTypeList) 
        {
            this.dbConnection = dbConnection;
            this.departmentList = departmentList;
            this.meetingTypeList = meetingTypeList;
        }   
        public IActionResult Index()
        {
            using(SqlConnection connection = dbConnection.CreateConnection())
            {
                ViewBag.DepartmentList = departmentList.GetDepartmentSelectList();
                ViewBag.MeetingTypeList = meetingTypeList.GetMeetingTypeSelectList();
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GET_ALL_MEETINGS";
                SqlDataReader reader = cmd.ExecuteReader();
                List<MeetingsModel> meetingsList = new List<MeetingsModel>();
                while (reader.Read())
                {
                    MeetingsModel meeting = new MeetingsModel();
                    meeting.MeetingID = Convert.ToInt32(reader["MeetingID"]);
                    meeting.DepartmentID = Convert.ToInt32(reader["DepartmentID"]);
                    meeting.DepartmentName = reader["DepartmentName"].ToString();
                    meeting.MeetingDate = Convert.ToDateTime(reader["MeetingDate"]);
                    meeting.MeetingDescription = reader["MeetingDescription"].ToString();
                    meeting.IsCancelled = Convert.ToBoolean(reader["IsCancelled"]);
                    meeting.DocumentPath = reader["DocumentPath"].ToString();
                    meeting.MeetingTypeName = reader["MeetingTypeName"].ToString();
                    meeting.MeetingVenueName = reader["MeetingVenueName"].ToString();

                    meetingsList.Add(meeting);

                }
                reader.Close();

            return View("Meeting_List",meetingsList);

            }
        }
        public IActionResult Create()
        {
            return View("Create_Meeting");
        }
        public IActionResult Save(MeetingsModel model)
        {
            if (!ModelState.IsValid)
            {
                return View ("Create_Meeting");
            }
            return View("Meeting_List");
        }

        public IActionResult Detail(int id)
        {
            using (SqlConnection connection = dbConnection.CreateConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GET_BY_ID_MEETINGS";
                cmd.Parameters.AddWithValue("@MeetingID", id);
                SqlDataReader reader = cmd.ExecuteReader();
                MeetingsModel meeting = new MeetingsModel();
                if (reader.Read())
                {
                    meeting.MeetingID = Convert.ToInt32(reader["MeetingID"]);
                    meeting.DepartmentID = Convert.ToInt32(reader["DepartmentID"]);
                    meeting.DepartmentName = reader["DepartmentName"].ToString();
                    meeting.MeetingDate = Convert.ToDateTime(reader["MeetingDate"]);
                    meeting.MeetingDescription = reader["MeetingDescription"].ToString();
                    meeting.CancellationDateTime = reader["CancellationDateTime"] == DBNull.Value ? null : Convert.ToDateTime(reader["CancellationDateTime"]);
                    meeting.CancellationReason = reader["CancellationReason"].ToString();
                    meeting.IsCancelled = Convert.ToBoolean(reader["IsCancelled"]);
                    meeting.DocumentPath = reader["DocumentPath"].ToString();
                    meeting.MeetingTypeName = reader["MeetingTypeName"].ToString();
                    meeting.MeetingVenueName = reader["MeetingVenueName"].ToString();
                }
                reader.Close();
                return View("Detail_Meeting", meeting);
            }
        }
    }
}
