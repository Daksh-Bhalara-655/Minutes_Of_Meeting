using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Minutes_Of_Meeting.DbConfig;
using Minutes_Of_Meeting.Models;
using System.Data;

namespace Minutes_Of_Meeting.Controllers
{
    public class MeetingVenueController : Controller
    {
        private readonly Db_Connection db_Connection;

        public MeetingVenueController (Db_Connection db_Connection)
        {
            this.db_Connection = db_Connection;
        }

         
        public IActionResult Index()
        {
            using (SqlConnection conn = db_Connection.CreateConnection())
            {
                List<MeetingVenue> venues = new List<MeetingVenue>();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SP_GET_ALL_MEETING_VENUES";
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    MeetingVenue venue = new MeetingVenue();
                    venue.MeetingVenueID = Convert.ToInt32(reader["MeetingVenueID"]);
                    venue.MeetingVenueName = reader["MeetingVenueName"].ToString();
                    venue.Created = Convert.ToDateTime(reader["Created"]);
                    venue.Modified = Convert.ToDateTime(reader["Modified"]);
                    venues.Add(venue);
                }
                return View("Meeting_Venue_List",venues);
            }
            
        }

        public IActionResult Create(int ? id)
        {
            MeetingVenue meetingVenue = new MeetingVenue();
            if(id != null && id > 0)
            {
                using (SqlConnection conn = db_Connection.CreateConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SP_GET_BY_ID_MEETING_VENUE";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MeetingVenueID", id);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        meetingVenue.MeetingVenueID = Convert.ToInt32(reader["MeetingVenueID"]);
                        meetingVenue.MeetingVenueName = reader["MeetingVenueName"].ToString();
                        meetingVenue.Created = Convert.ToDateTime(reader["Created"]);
                        meetingVenue.Modified = Convert.ToDateTime(reader["Modified"]);
                    }
                    reader.Close();
                }
            }
            return View("Create_Meeting_Venue" , meetingVenue);
        }

        public IActionResult Save(MeetingVenue model)
        {
            if (!ModelState.IsValid)
            {
                return View("Create_Meeting_Venue", model);
            }

            using (SqlConnection conn = db_Connection.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                if (model.MeetingVenueID != null && model.MeetingVenueID > 0)
                {
                    cmd.CommandText = "SP_UPDATE_MEETING_VENUE";
                    cmd.CommandType= CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MeetingVenueID", model.MeetingVenueID);

                }
                else
                {
                    cmd.CommandText = "SP_INSERT_MEETING_VENUE";
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                cmd.Parameters.AddWithValue("@MeetingVenueName", model.MeetingVenueName);
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
                cmd.CommandText = "SP_DELETE_MEETING_VENUE";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MeetingVenueID", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
       
        }
}
