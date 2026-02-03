using System.ComponentModel.DataAnnotations;

namespace Minutes_Of_Meeting.Models
{
    public class MeetingVenue
    {
        [Key]
        public int MeetingVenueID { get; set; }

        [Required(ErrorMessage = "Meeting Venue Name is required")]
        public string MeetingVenueName { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
