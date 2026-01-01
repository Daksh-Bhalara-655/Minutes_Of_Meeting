namespace Minutes_Of_Meeting.Models
{
    public class MeetingMembers
    {
        public int Meeting_Member_Id { get; set; }

        public int Meeting_Id { get; set; }

        public int Staff_Id { get; set; }

        public bool Is_Present { get; set; }

        public string? Remark { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
