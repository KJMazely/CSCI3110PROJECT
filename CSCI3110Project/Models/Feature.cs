using KMCSCI3110Project.Models;
using System.ComponentModel.DataAnnotations;

public class Feature
{
    public int Id { get; set; }
    [Required, StringLength(100)]
    public string Name { get; set; }

    public ICollection<VehicleFeature> VehicleFeatures { get; set; }
      = new List<VehicleFeature>();
}