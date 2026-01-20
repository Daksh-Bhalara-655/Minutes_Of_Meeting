using System.ComponentModel.DataAnnotations;

namespace Minutes_Of_Meeting.Models
{
    public class MeetingMembers
    {
        [Key]
        public int Meeting_Member_Id { get; set; }

        [Key]
        public int Meeting_Id { get; set; }

        [Key]
        public int Staff_Id { get; set; }


        public bool Is_Present { get; set; }

        public string? Remark { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
