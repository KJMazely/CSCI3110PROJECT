// Main table for all vehicles

using KMCSCI3110Project.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KMCSCI3110Project.Data.Repositories
{
    public interface IVehicleRepository
    {
        Task<List<Vehicle>> GetAllAsync();
        Task<Vehicle> GetByIdAsync(int id);
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(int id);
        Task<List<Feature>> ReadAllFeaturesAsync(); 
    }
}
