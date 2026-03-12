using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Minutes_Of_Meeting.DbConfig;
using Minutes_Of_Meeting.Models;
using Minutes_Of_Meeting.Services;
using OfficeOpenXml;
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
                    attendance.AttendancePercent = Convert.ToDecimal(reader["AttendancePercent"]);
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

        [HttpPost]
        public IActionResult SaveAttendance(int MeetingId, List<int> PresentStaff)
        {
            using (SqlConnection conn = db_Connection.CreateConnection())
            {
                conn.Open();

                DataTable attendanceTable = new DataTable();
                attendanceTable.Columns.Add("StaffID", typeof(int));
                attendanceTable.Columns.Add("IsPresent", typeof(bool));

                foreach (var staff in PresentStaff)
                {
                    attendanceTable.Rows.Add(staff, true);
                }

                SqlCommand cmd = new SqlCommand("SP_UPDATE_MEETING_ATTENDANCE", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MeetingID", MeetingId);

                SqlParameter param = cmd.Parameters.AddWithValue("@AttendanceList", attendanceTable);
                param.SqlDbType = SqlDbType.Structured;
                param.TypeName = "AttendanceTableType";

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index", new { id = MeetingId });
        }

        public IActionResult ExportAttendanceExcel(int id, DateTime MeetingDate)
        {
            ExcelPackage.License.SetNonCommercialPersonal("DAKSH BHALARA");

            List<MeetingAttendanceModel> staffList = new List<MeetingAttendanceModel>();

            using (SqlConnection conn = db_Connection.CreateConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SP_GET_MEETING_MEMBERS_WITH_ATTENDANCE", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MEETINGID", id);
                cmd.Parameters.AddWithValue("@MeetingDate", MeetingDate);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    staffList.Add(new MeetingAttendanceModel
                    {
                        StaffID = Convert.ToInt32(reader["StaffID"]),
                        StaffName = reader["StaffName"].ToString(),
                        DepartmentName = reader["DepartmentName"].ToString(),
                        IsPresent = Convert.ToBoolean(reader["IsPresent"]),
                        AttendancePercent = Convert.ToDecimal(reader["AttendancePercent"])
                    });
                }
            }

            using (var package = new ExcelPackage())
            {
                var sheet = package.Workbook.Worksheets.Add("Attendance");

                sheet.Cells[1, 1].Value = "Staff ID";
                sheet.Cells[1, 2].Value = "Staff Name";
                sheet.Cells[1, 3].Value = "Department";
                sheet.Cells[1, 4].Value = "Attendance %";
                sheet.Cells[1, 5].Value = "Status";

                int row = 2;

                foreach (var staff in staffList)
                {
                    sheet.Cells[row, 1].Value = "WMP00" + staff.StaffID;
                    sheet.Cells[row, 2].Value = staff.StaffName;
                    sheet.Cells[row, 3].Value = staff.DepartmentName;
                    sheet.Cells[row, 4].Value = staff.AttendancePercent;
                    sheet.Cells[row, 5].Value = staff.IsPresent ? "Present" : "Absent";
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "MeetingAttendance.xlsx");
            }
        }
    }

}