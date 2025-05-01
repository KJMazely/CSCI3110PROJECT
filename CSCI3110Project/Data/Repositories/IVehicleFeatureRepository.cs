// Many to Many for vehicle features
using KMCSCI3110Project.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KMCSCI3110Project.Data.Repositories
{
    public interface IVehicleFeatureRepository
    {
        Task<List<Feature>> ReadAllFeaturesAsync();
        Task<List<int>> ReadFeatureIdsByVehicleAsync(int vehicleId);
        Task AssignFeatureToVehicleAsync(int vehicleId, int featureId);
        Task RemoveFeatureFromVehicleAsync(int vehicleId, int featureId);
    }
}
