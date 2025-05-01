// Main table repo for all vehicles

using KMCSCI3110Project.Data;
using KMCSCI3110Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KMCSCI3110Project.Data.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _db;

        public VehicleRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Vehicle>> GetAllAsync()
            => await _db.Vehicles.ToListAsync();

        public async Task<Vehicle> GetByIdAsync(int id)
            => await _db.Vehicles.FindAsync(id);

        public async Task<List<Feature>> ReadAllFeaturesAsync()
        {
            return await _db.Features
                            .OrderBy(f => f.Name)
                            .ToListAsync();
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            _db.Vehicles.Add(vehicle);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            _db.Vehicles.Update(vehicle);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var v = await GetByIdAsync(id);
            if (v != null)
            {
                _db.Vehicles.Remove(v);
                await _db.SaveChangesAsync();
            }
        }
    }
}
