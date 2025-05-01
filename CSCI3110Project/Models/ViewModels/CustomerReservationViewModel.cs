using System;

namespace KMCSCI3110Project.Models.ViewModels
{
    public class CustomerReservationViewModel
    {
        public int Id { get; set; }
        public string VehicleName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalCost { get; set; }
    }
}
