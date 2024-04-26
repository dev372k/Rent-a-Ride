namespace Presentation.Models
{
    public class DashboardModel
    {
        public int ActiveUsers { get; set; }     
        public int DisableUsers { get; set; }     
        public int TotalsUsers { get; set; }
        public int TotalBookings { get; set; }
        public int ActiveBookings { get; set; }
        public double TotalRevenue { get; set; }
    }
}
