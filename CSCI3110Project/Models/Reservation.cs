using System;
using System.ComponentModel.DataAnnotations;

namespace KMCSCI3110Project.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal TotalCost { get; set; }
    }
}