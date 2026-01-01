namespace Minutes_Of_Meeting.Models
{
    public class Staff
    {
        public int Staff_Id { get; set; }

        public int Department_Id { get; set; }

        public string Staff_Name { get; set; }

        public string Email_Address { get; set; }

        public string? Remark { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
