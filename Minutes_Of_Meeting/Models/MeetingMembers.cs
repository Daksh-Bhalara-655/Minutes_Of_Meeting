using System.ComponentModel.DataAnnotations;

namespace Minutes_Of_Meeting.Models
{
    public class MeetingMembers
    {
        [Key]
        public int MeetingMemberID { get; set; }

        [Key]
        public int MeetingID { get; set; }

        [Key]
        public int StaffID { get; set; }


        public bool IsPresent { get; set; }

        public string? Remark { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
