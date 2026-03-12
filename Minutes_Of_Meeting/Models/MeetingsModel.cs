using System;
using System.ComponentModel.DataAnnotations;

namespace Minutes_Of_Meeting.Models
{
    public class MeetingsModel
    {
        public int MeetingID { get; set; }

        [Required]
        public int DepartmentID { get; set; }

        public string? DepartmentName { get; set; }

        [Required(ErrorMessage = "Meeting date is required")]
        [DataType(DataType.Date)]
        public DateTime MeetingDate { get; set; } 

        [Required(ErrorMessage = "Meeting description is required")]
        [StringLength(250)]
        public string MeetingDescription { get; set; }

        public bool IsCancelled { get; set; }

        public string? DocumentPath { get; set; }

        [Required(ErrorMessage = "Meeting type is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a meeting type")]
        public int? MeetingTypeId { get; set; }

        [Required(ErrorMessage = "Meeting venue is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a meeting venue")]
        public int? MeetingVenueId { get; set; }

        public string? MeetingTypeName { get; set; }
        public string? MeetingVenueName { get; set; }

        public string? CancellationReason { get; set; }
        public DateTime? CancellationDateTime { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; } = DateTime.Now;
    }
}