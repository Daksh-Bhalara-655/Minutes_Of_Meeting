namespace Minutes_Of_Meeting.Models
{
    public class MeetingsModel  
    {
        public int Meeting_Id { get; set; }

        public DateTime Meeting_Date { get; set; }

        public int Meeting_Venue_Id { get; set; }

        public int Meeting_Type_ID { get; set; }
        //DepartmentID
        public int Department_ID { get; set; }
        // MeetingDescription
        public string ? Meeting_Description { get; set; }

        public string? Document_Path { get; set; }

        public DateTime Created_At { get; set; }
        public DateTime? Updated_At { get;set;}   

        public bool Is_cancelled { get; set; }

        public string? Cancellation_Reason { get; set; }
        public  DateTime? CancellationDateTime { get; set; }


    }
}
