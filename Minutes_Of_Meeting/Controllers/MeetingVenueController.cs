using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Minutes_Of_Meeting.Models;
using System.Data;

namespace Minutes_Of_Meeting.Controllers
{
    public class MeetingVenueController : Controller
    {
        public IActionResult Index()
        {
            List<MeetingVenue> venues = new List<MeetingVenue>();
            SqlConnection conn = new SqlConnection("Server=LAPTOP-9V1759OU\\SQLEXPRESS;Database=Minutes_of_Meeting_Management;Trusted_Connection=True;TrustServerCertificate=True");
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

        public IActionResult Create()
        {
            return View("Create_Meeting_Venue");
        }

        public IActionResult Save(MeetingVenue model)
        {
            if (!ModelState.IsValid)
            {
                return View("Create_Meeting_Venue", model);
            }
            return View("Meeting_Venue_List");
        }
    }
}
