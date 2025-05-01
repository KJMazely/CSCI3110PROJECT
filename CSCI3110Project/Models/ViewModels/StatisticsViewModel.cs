namespace KMCSCI3110Project.ViewModels
{
    public class StatisticsViewModel
    {
        public int TotalVehicles { get; set; }
        public int AvailableVehicles { get; set; }
        public int UnavailableVehicles { get; set; }
        public decimal AverageRentalCost { get; set; }
        public int TotalReservations { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalInquiries { get; set; }
        public int TotalAccounts { get; set; }
    }
}
