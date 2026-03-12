using System.Collections.Generic;

namespace Minutes_Of_Meeting.Models
{
    public class DashboardViewModel
    {
        public int TotalMeetings { get; set; }
        public int UpcomingMeetings { get; set; }
        public int CompletedMeetings { get; set; }
        public int CancelledMeetings { get; set; }

        public List<ChartData> TypeData { get; set; } = new();
        public List<ChartData> DeptData { get; set; } = new();
        public List<TrendData> TrendData { get; set; } = new();
    }
}