using System.ComponentModel.DataAnnotations;

namespace Minutes_Of_Meeting.Models
{
    public class Staff
    {
        [Key]
        public int Staff_Id { get; set; }
        [Key]
        public int Department_Id { get; set; }
        [Required(ErrorMessage = "Staff_Name is required")]
        public string Staff_Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email_Address { get; set; }

        public string? Remark { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
