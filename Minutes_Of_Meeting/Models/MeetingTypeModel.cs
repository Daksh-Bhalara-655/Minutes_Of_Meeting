using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;

namespace Minutes_Of_Meeting.Models
{
    public class MeetingTypeModel
    {
        [Key]
        public int Meeting_Id { get; set; }

        [Required(ErrorMessage = "Meeting-Type-Name is required")]
        public string Meeting_Type_Name { get; set; }

        [StringLength (100)]
        public string? Remarks { get; set;  }
        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

    }
}
