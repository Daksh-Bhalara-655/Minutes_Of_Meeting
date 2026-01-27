using System.ComponentModel.DataAnnotations;

namespace Minutes_Of_Meeting.Models
{
    public class DepartmentModel
    {
        [Key]
        public int Department_Id { get; set; }

        [Required(ErrorMessage = "Department Name is required")]
        public string Department_Name { get; set; }

        [Display(Name="Created Date")]
      
        public DateTime Created { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime Modified { get; set; }
    }
}
