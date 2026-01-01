using Microsoft.AspNetCore.Http.HttpResults;

namespace Minutes_Of_Meeting.Models
{
    public class MeetingTypeModel
    {
        public int Meeting_Id { get; set; }

        public string Meeting_Type_Name { get; set; }
        public string? Remarks { get; set;  }
        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

    }
}
