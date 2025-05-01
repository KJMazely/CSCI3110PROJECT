using System;

namespace KMCSCI3110Project.Models.ViewModels
{
    public class ReservationAdminViewModel
    {
        public int ReservationId { get; set; }
        public string CustomerEmail { get; set; }
        public int VehicleId { get; set; }
        public string VehicleName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalCost { get; set; }
    }
}
