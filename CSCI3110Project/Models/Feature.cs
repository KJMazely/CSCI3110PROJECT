using System.ComponentModel.DataAnnotations;

namespace KMCSCI3110Project.Models
{
    public class Feature
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public ICollection<VehicleFeature> VehicleFeatures { get; set; }
            = new List<VehicleFeature>();
    }
}
