using Microsoft.AspNetCore.Mvc.Rendering;
using Minutes_Of_Meeting.Models;

public class StaffListViewModel
{
    public List<Staff> Staffs { get; set; }

    public List<SelectListItem> DepartmentList { get; set; }
}
