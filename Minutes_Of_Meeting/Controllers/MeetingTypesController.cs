using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Minutes_Of_Meeting.DbConfig;
using Minutes_Of_Meeting.Models;
using System.Data;
using System.Data.Common;

namespace Minutes_Of_Meeting.Controllers
{
    public class MeetingTypesController : Controller
    {
        private readonly Db_Connection db_Connection;

        public MeetingTypesController(Db_Connection  db_Connection)
        {
            this.db_Connection = db_Connection;
        }
        public IActionResult Index()
        {
            using (SqlConnection conn = db_Connection.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SP_GET_ALL_MEETING_TYPES";
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                DbDataReader reader = cmd.ExecuteReader();
                List<MeetingTypeModel> meetingTypes = new List<MeetingTypeModel>();
                while (reader.Read())
                {
                    MeetingTypeModel meetingType = new MeetingTypeModel();
                    meetingType.MeetingTypeID = Convert.ToInt32(reader["MeetingTypeID"]);
                    meetingType.MeetingTypeName = reader["MeetingTypeName"].ToString();
                    meetingType.Created = Convert.ToDateTime(reader["created"]);
                    meetingType.Modified = Convert.ToDateTime(reader["modified"]);
                    meetingTypes.Add(meetingType);
                }
                reader.Close();
            return View("Meeting_Type_List",meetingTypes);
            }
        }
        public IActionResult Create(int? id)
        {
             MeetingTypeModel meetingTypeModel = new MeetingTypeModel();
                if (id != null && id > 0)
                {
                    using (SqlConnection conn = db_Connection.CreateConnection())
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = "SP_GET_MEETING_TYPE_BY_ID";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MeetingTypeID", id);
                        conn.Open();
                        DbDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            meetingTypeModel.MeetingTypeID = Convert.ToInt32(reader["MeetingTypeID"]);
                            meetingTypeModel.MeetingTypeName = reader["MeetingTypeName"].ToString();
                            meetingTypeModel.Remarks = reader["Remarks"].ToString();
                            meetingTypeModel.Created = Convert.ToDateTime(reader["created"]);
                            meetingTypeModel.Modified = Convert.ToDateTime(reader["modified"]);
                        }
                        reader.Close();
                    }
            }
                return View("Create_Meeting_Type", meetingTypeModel);

        }
        public IActionResult Save(MeetingTypeModel model)
        {
                if (!ModelState.IsValid)
                {
                    return View("Create_Meeting_Type", model);
                }

               using (SqlConnection conn = db_Connection.CreateConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    if (model.MeetingTypeID > 0)
                    {
                        cmd.CommandText = "SP_UPDATE_MEETING_TYPE";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MeetingTypeID", model.MeetingTypeID);

                    }
                    else
                    {
                      
                        cmd.CommandText = "SP_INSERT_MEETING_TYPE";
                        cmd.CommandType = CommandType.StoredProcedure;

                    }
                    cmd.Parameters.AddWithValue("@MeetingTypeName", model.MeetingTypeName);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                    conn.Open();
                    cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            using (SqlConnection conn = db_Connection.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SP_DELETE_MEETING_TYPE";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MeetingTypeID", id);
                conn.Open();
                cmd.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
        }
    }
}
