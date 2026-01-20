using System.ComponentModel.DataAnnotations;

namespace Minutes_Of_Meeting.Models
{
    public class MeetingVenue
    {
        [Key]
        public int Meeting_Venue_Id { get; set; }

        [Required(ErrorMessage = "Meeting Venue Name is required")]
        public string Meeting_Venue_Name { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
