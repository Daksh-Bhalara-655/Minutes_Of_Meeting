using System;
using System.ComponentModel.DataAnnotations;

namespace Minutes_Of_Meeting.Models
{
    public class MeetingsModel
    {
        public int MeetingID { get; set; }

        [Required]
        public int DepartmentID { get; set; }

        public string DepartmentName { get; set; }

        [Required(ErrorMessage = "Meeting date is required")]
        [DataType(DataType.Date)]
        public DateTime MeetingDate { get; set; }

        [Required(ErrorMessage = "Meeting description is required")]
        [StringLength(500)]
        public string MeetingDescription { get; set; }

        public bool IsCancelled { get; set; }

        public string? DocumentPath { get; set; }

        public string? MeetingTypeName { get; set; }

        public string? MeetingVenueName { get; set; }

        public string? CancellationReason { get; set; }

        public string? CancellationReasonDescription { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; } = DateTime.MinValue;

        public DateTime? CancellationDateTime { get; set; }

    }
}