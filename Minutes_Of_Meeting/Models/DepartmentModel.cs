using System.ComponentModel.DataAnnotations;

namespace Minutes_Of_Meeting.Models
{
    public class DepartmentModel
    {
        [Key]
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "Department Name is required")]
        public string DepartmentName { get; set; }

        [Display(Name="Created Date")]
      
        public DateTime? Created { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? Modified { get; set; }
    }
}
