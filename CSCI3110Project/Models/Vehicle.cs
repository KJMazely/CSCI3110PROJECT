using System.ComponentModel.DataAnnotations;

namespace KMCSCI3110Project.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Make { get; set; }

        [Required, StringLength(50)]
        public string Model { get; set; }

        [Range(1900, 2100)]
        public int Year { get; set; }

        [Required, DataType(DataType.Currency)]
        public decimal RentalCostPerDay { get; set; }

        public int Mileage { get; set; }
        public string ImageUrl { get; set; }

        [Required, Display(Name = "Class")]
        public string VehicleClass { get; set; }
        public string Size { get; set; }
        public bool IsAvailable { get; set; }

        [Range(1, 15)]
        public int SeatAmount { get; set; }

        [Range(1, 10)]
        public int DoorCount { get; set; }

        [Range(0, 150)]
        public int MPG { get; set; }

        [Range(0, 50)]
        public int CargoSize { get; set; }

        [Required, StringLength(20)]
        public string Gearbox { get; set; }


        // Many-to-many
        public ICollection<VehicleFeature> VehicleFeatures { get; set; }
            = new List<VehicleFeature>();

    }
}
