using System.ComponentModel.DataAnnotations;

namespace Minutes_Of_Meeting.Models
{
    public class MeetingAttendanceModel
    {
        [Key]
        public int StaffID { get; set; }
        [Required]
        public string StaffName { get; set; }

        [Required]
        public string DepartmentName { get; set; }


        [Required]
        public bool IsPresent { get; set; }

        [Required]
        public string MeetingVenueName { get; set; }

        public string? MemberRemarks { get; set; }

        [Required]
        public string MeetingTypeName { get; set; }

    }
}
