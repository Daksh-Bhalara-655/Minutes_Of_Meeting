using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;

namespace Minutes_Of_Meeting.Models
{
    public class MeetingTypeModel
    {
        [Key]
        public int MeetingTypeID { get; set; }

        [Required(ErrorMessage = "Meeting-Type-Name is required")]
        public string MeetingTypeName { get; set; }

        [StringLength (100)]
        public string? Remarks { get; set;  }
        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

    }
}
