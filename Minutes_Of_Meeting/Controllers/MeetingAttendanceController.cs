using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Minutes_Of_Meeting.DbConfig;
using Minutes_Of_Meeting.Models;
using Minutes_Of_Meeting.Services;
using System.Data;

namespace Minutes_Of_Meeting.Controllers
{
    public class MeetingAttendanceController : Controller
    {
        private readonly Db_Connection db_Connection;
        private readonly DepartmentList departmentList;
        private readonly MeetingTypeList meetingTypeList;

        public MeetingAttendanceController(Db_Connection db_Connection, DepartmentList departmentList, MeetingTypeList meetingTypeList)
        {
            this.db_Connection = db_Connection;
            this.departmentList = departmentList;
            this.meetingTypeList = meetingTypeList;
        }


        public IActionResult Index(int? id, DateOnly? MeetingDate)
        {

            ViewBag.DepartmentList = departmentList.GetDepartmentSelectList();
            ViewBag.MeetingTypeList = meetingTypeList.GetMeetingTypeSelectList();

            List<MeetingAttendanceModel> meetingAttendances = new List<MeetingAttendanceModel>();

            using (SqlConnection conn = db_Connection.CreateConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SP_GET_MEETING_MEMBERS_WITH_ATTENDANCE", conn);
                cmd.CommandType = CommandType.StoredProcedure;


                if (id.HasValue)
                    cmd.Parameters.AddWithValue("@MEETINGID", id.Value);
                else
                    cmd.Parameters.AddWithValue("@MEETINGID", DBNull.Value);
                if (MeetingDate.HasValue)
                    cmd.Parameters.AddWithValue("@MeetingDate", MeetingDate.Value.ToDateTime(TimeOnly.MinValue));
                else
                    cmd.Parameters.AddWithValue("@MeetingDate", DBNull.Value);


                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MeetingAttendanceModel attendance = new MeetingAttendanceModel();
                    attendance.StaffID = Convert.ToInt32(reader["StaffID"]);
                    attendance.StaffName = reader["StaffName"].ToString();
                    attendance.DepartmentName = reader["DepartmentName"].ToString();
                    attendance.IsPresent = Convert.ToBoolean(reader["IsPresent"]);
                    attendance.MeetingVenueName = reader["MeetingVenueName"].ToString();
                    attendance.MemberRemarks = reader["MemberRemarks"] != DBNull.Value ? reader["MemberRemarks"].ToString() : null;
                    attendance.MeetingTypeName = reader["MeetingTypeName"].ToString();
                    attendance.MeetingDate = Convert.ToDateTime(reader["MeetingDate"]);
                    meetingAttendances.Add(attendance);
                }
                reader.Close();
                conn.Close();
            }


            return View("Meeting_Attendance", meetingAttendances);
        }
        [HttpPost]
        public IActionResult Filter(int? id, DateOnly? MeetingDate)
        {
            return RedirectToAction("Index", new { id = id, MeetingDate = MeetingDate });
        }
    }

}