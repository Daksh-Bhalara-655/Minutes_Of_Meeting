using Microsoft.AspNetCore.Mvc.Rendering;

namespace Minutes_Of_Meeting.Models
{
    public class AttendanceViewModel
    {
       public List<MeetingAttendanceModel> MeetingAttendances { get; set; }
       public List<SelectListItem> DepartmentList { get; set; }
       public List<SelectListItem> MeetingTypesList { get; set; }

       public List<Staff> Staff { get; set; }
    }
}
