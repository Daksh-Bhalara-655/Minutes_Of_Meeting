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
        private readonly MeetingVenueList meetingVenueList;

        public MeetingsController(Db_Connection dbConnection, DepartmentList departmentList , MeetingTypeList meetingTypeList , MeetingVenueList meetingVenueList) 
        {
            this.dbConnection = dbConnection;
            this.departmentList = departmentList;
            this.meetingTypeList = meetingTypeList;
            this.meetingVenueList = meetingVenueList;
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
                cmd.CommandText = "SP_GET_ALL_todays_meetings_and_future_meetings";
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    List<MeetingsModel> meetingsList = new List<MeetingsModel>();

                    // Check which columns are present in the result set to avoid IndexOutOfRange
                    bool hasVenue = false, hasType = false, hasDepartmentName = false, hasTypeName = false, hasVenueName = false;
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var name = reader.GetName(i);
                        if (string.Equals(name, "MeetingVenueId", StringComparison.OrdinalIgnoreCase)) hasVenue = true;
                        if (string.Equals(name, "MeetingTypeId", StringComparison.OrdinalIgnoreCase)) hasType = true;
                        if (string.Equals(name, "DepartmentName", StringComparison.OrdinalIgnoreCase)) hasDepartmentName = true;
                        if (string.Equals(name, "MeetingTypeName", StringComparison.OrdinalIgnoreCase)) hasTypeName = true;
                        if (string.Equals(name, "MeetingVenueName", StringComparison.OrdinalIgnoreCase)) hasVenueName = true;
                    }

                    while (reader.Read())
                    {
                        MeetingsModel meeting = new MeetingsModel();
                        meeting.MeetingID = reader["MeetingID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["MeetingID"]);
                        meeting.DepartmentID = reader["DepartmentID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["DepartmentID"]);
                        meeting.MeetingDate = reader["MeetingDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["MeetingDate"]);
                        meeting.MeetingDescription = reader["MeetingDescription"] == DBNull.Value ? string.Empty : reader["MeetingDescription"].ToString();
                        meeting.IsCancelled = reader["IsCancelled"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsCancelled"]);
                        meeting.DocumentPath = reader["DocumentPath"] == DBNull.Value ? null : reader["DocumentPath"].ToString();
                        meeting.DepartmentName = reader["DepartmentName"].ToString();
                        meeting.MeetingVenueName = reader["MeetingVenueName"].ToString();
                        meeting.MeetingTypeName = reader["MeetingTypeName"].ToString();

                        meeting.MeetingVenueId = hasVenue ? (reader["MeetingVenueId"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["MeetingVenueId"])) : null;
                        meeting.MeetingTypeId  = hasType  ? (reader["MeetingTypeId"]  == DBNull.Value ? null : (int?)Convert.ToInt32(reader["MeetingTypeId"]))  : null;

                        meeting.DepartmentName   = hasDepartmentName ? (reader["DepartmentName"] == DBNull.Value ? string.Empty : reader["DepartmentName"].ToString()) : null;
                        meeting.MeetingTypeName  = hasTypeName       ? (reader["MeetingTypeName"] == DBNull.Value ? string.Empty : reader["MeetingTypeName"].ToString()) : null;
                        meeting.MeetingVenueName = hasVenueName      ? (reader["MeetingVenueName"] == DBNull.Value ? string.Empty : reader["MeetingVenueName"].ToString()) : null;

                        meetingsList.Add(meeting);
                    }

                    return View("Meeting_List", meetingsList);
                }
            }
        }
        // The Create action serves both the Create and Edit views. If an ID is provided, it loads the existing meeting data for editing; otherwise, it returns an empty model for creating a new meeting.
        public IActionResult Create(int? id)
        {
            ViewBag.DepartmentList = departmentList.GetDepartmentSelectList();
            ViewBag.MeetingTypeList = meetingTypeList.GetMeetingTypeSelectList();
            ViewBag.MeetingvenueList = meetingVenueList.GetMeetingVenueSelectList();

            MeetingsModel meeting = new MeetingsModel();

            if (id.HasValue && id > 0)
            {
                using (SqlConnection connection = dbConnection.CreateConnection())
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("SP_GET_BY_ID_MEETINGS", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MeetingID", id.Value);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        meeting.MeetingID = Convert.ToInt32(reader["MeetingID"]);
                        meeting.DepartmentID = Convert.ToInt32(reader["DepartmentID"]);
                        meeting.MeetingTypeId = Convert.ToInt32(reader["MeetingTypeID"]);
                        meeting.MeetingVenueId = Convert.ToInt32(reader["MeetingVenueID"]);
                        meeting.MeetingDate = Convert.ToDateTime(reader["MeetingDate"]);
                        meeting.MeetingDescription = reader["MeetingDescription"].ToString();
                        meeting.IsCancelled = reader["IsCancelled"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsCancelled"]);
                        meeting.DocumentPath = reader["DocumentPath"].ToString();

                        meeting.CancellationDateTime =
                            reader["CancellationDateTime"] == DBNull.Value
                            ? null
                            : Convert.ToDateTime(reader["CancellationDateTime"]);

                        meeting.CancellationReason = reader["CancellationReason"].ToString();
                    }

                    reader.Close();
                }
            }

            return View("Create_Meeting", meeting);
        }
        
        // The Save action handles both Create and Edit operations based on whether MeetingID is present in the model.
        [HttpPost]
        public IActionResult Save(MeetingsModel model, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DepartmentList = departmentList.GetDepartmentSelectList();
                ViewBag.MeetingTypeList = meetingTypeList.GetMeetingTypeSelectList();
                ViewBag.MeetingvenueList = meetingVenueList.GetMeetingVenueSelectList();
                return View("Create_Meeting", model);
            }

            try
            {
                // 1. Handle File Upload
                if (file != null && file.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    model.DocumentPath = "/uploads/" + fileName;
                }
              
                using (SqlConnection connection = dbConnection.CreateConnection())
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (model.MeetingID > 0)
                    {
                        // EDIT MODE
                        cmd.CommandText = "SP_UPDATE_MEETING";
                        cmd.Parameters.Add("@MeetingID", SqlDbType.Int).Value = model.MeetingID;
                    }
                    else
                    {
                        cmd.CommandText = "SP_INSERT_MEETING";
                        var outParam = new SqlParameter("@NewMeetingID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(outParam);
                    }

                    cmd.Parameters.Add("@MeetingDate", SqlDbType.DateTime).Value = model.MeetingDate; // Changed to DateTime to keep time data
                    cmd.Parameters.Add("@MeetingTypeID", SqlDbType.Int).Value = (object?)model.MeetingTypeId ?? DBNull.Value;
                    cmd.Parameters.Add("@MeetingVenueID", SqlDbType.Int).Value = (object?)model.MeetingVenueId ?? DBNull.Value;
                    cmd.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = model.DepartmentID;
                    cmd.Parameters.Add("@MeetingDescription", SqlDbType.NVarChar, 250).Value = string.IsNullOrEmpty(model.MeetingDescription) ? DBNull.Value : model.MeetingDescription;
                    cmd.Parameters.Add("@DocumentPath", SqlDbType.NVarChar, 250).Value = string.IsNullOrEmpty(model.DocumentPath) ? DBNull.Value : model.DocumentPath;

                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content("ERROR: " + ex.Message);
            }
        }

        public IActionResult Detail(int id)
        {
            MeetingsModel meeting = new MeetingsModel();

            using (SqlConnection conn = dbConnection.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_GET_BY_ID_MEETINGS", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MEETINGID", id);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    meeting.MeetingID = reader["MeetingID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["MeetingID"]);

                    meeting.DepartmentID = reader["DepartmentID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["DepartmentID"]);
                    meeting.DepartmentName = reader["DepartmentName"]?.ToString();

                    meeting.MeetingDate = reader["MeetingDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["MeetingDate"]);

                    meeting.MeetingDescription = reader["MeetingDescription"]?.ToString();

                    meeting.IsCancelled = reader["IsCancelled"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsCancelled"]);

                    meeting.DocumentPath = reader["DocumentPath"]?.ToString();

                    // ✅ IDs
                    meeting.MeetingTypeId = reader["MeetingTypeID"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["MeetingTypeID"]);

                    meeting.MeetingVenueId = reader["MeetingVenueID"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["MeetingVenueID"]);

                    // ✅ Names
                    meeting.MeetingTypeName = reader["MeetingTypeName"]?.ToString();
                    meeting.MeetingVenueName = reader["MeetingVenueName"]?.ToString();

                    // ✅ Cancellation
                    meeting.CancellationReason = reader["CancellationReason"]?.ToString();

                    meeting.CancellationDateTime = reader["CancellationDateTime"] == DBNull.Value
                        ? null
                        : (DateTime?)Convert.ToDateTime(reader["CancellationDateTime"]);
                }

                conn.Close();
            }

            return View("Detail_Meeting", meeting);
        }
        // Detail_Meeting
        public IActionResult History()
        {
            using (SqlConnection connection = dbConnection.CreateConnection())
            {
                ViewBag.DepartmentList = departmentList.GetDepartmentSelectList();
                ViewBag.MeetingTypeList = meetingTypeList.GetMeetingTypeSelectList();
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GET_ALL_MEETINGS";
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    List<MeetingsModel> meetingsList = new List<MeetingsModel>();

                    bool hasVenue = false, hasType = false, hasDepartmentName = false, hasTypeName = false, hasVenueName = false;
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var name = reader.GetName(i);
                        if (string.Equals(name, "MeetingVenueId", StringComparison.OrdinalIgnoreCase)) hasVenue = true;
                        if (string.Equals(name, "MeetingTypeId", StringComparison.OrdinalIgnoreCase)) hasType = true;
                        if (string.Equals(name, "DepartmentName", StringComparison.OrdinalIgnoreCase)) hasDepartmentName = true;
                        if (string.Equals(name, "MeetingTypeName", StringComparison.OrdinalIgnoreCase)) hasTypeName = true;
                        if (string.Equals(name, "MeetingVenueName", StringComparison.OrdinalIgnoreCase)) hasVenueName = true;
                    }

                    while (reader.Read())
                    {
                        MeetingsModel meeting = new MeetingsModel();
                        meeting.MeetingID = reader["MeetingID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["MeetingID"]);
                        meeting.DepartmentID = reader["DepartmentID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["DepartmentID"]);
                        meeting.MeetingDate = reader["MeetingDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["MeetingDate"]); meeting.MeetingDescription = reader["MeetingDescription"] == DBNull.Value ? string.Empty : reader["MeetingDescription"].ToString();
                        meeting.IsCancelled = reader["IsCancelled"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsCancelled"]);
                        meeting.DocumentPath = reader["DocumentPath"] == DBNull.Value ? null : reader["DocumentPath"].ToString();

                        meeting.MeetingVenueId = hasVenue ? (reader["MeetingVenueId"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["MeetingVenueId"])) : null;
                        meeting.MeetingTypeId = hasType ? (reader["MeetingTypeId"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["MeetingTypeId"])) : null;

                        meeting.DepartmentName = hasDepartmentName ? (reader["DepartmentName"] == DBNull.Value ? string.Empty : reader["DepartmentName"].ToString()) : null;
                        meeting.MeetingTypeName = hasTypeName ? (reader["MeetingTypeName"] == DBNull.Value ? string.Empty : reader["MeetingTypeName"].ToString()) : null;
                        meeting.MeetingVenueName = hasVenueName ? (reader["MeetingVenueName"] == DBNull.Value ? string.Empty : reader["MeetingVenueName"].ToString()) : null;

                        meetingsList.Add(meeting);
                    }

                    return View("Meeting_History_List", meetingsList);
                }
            }
        }
    }
}
