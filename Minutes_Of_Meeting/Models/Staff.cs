using System.ComponentModel.DataAnnotations;

namespace Minutes_Of_Meeting.Models
{
    public class Staff
    {
        [Key]
        public int StaffID { get; set; }
        
        public string DepartmentName { get; set; }
        [Required(ErrorMessage = "Staff_Name is required")]
        public string StaffName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string EmailAddress { get; set; }

        public string MobileNo { get; set; }

        public string? Remarks { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
