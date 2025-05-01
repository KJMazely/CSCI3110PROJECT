// Many to Many relationship of vehicle and features

using KMCSCI3110Project.Data;
using KMCSCI3110Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KMCSCI3110Project.Data.Repositories
{
    public class VehicleFeatureRepository : IVehicleFeatureRepository
    {
        private readonly ApplicationDbContext _db;
        public VehicleFeatureRepository(ApplicationDbContext db)
            => _db = db;

        public async Task<List<Feature>> ReadAllFeaturesAsync()
            => await _db.Features.ToListAsync();

        public async Task<List<int>> ReadFeatureIdsByVehicleAsync(int vehicleId)
            => await _db.VehicleFeatures
                        .Where(vf => vf.VehicleId == vehicleId)
                        .Select(vf => vf.FeatureId)
                        .ToListAsync();

        public async Task AssignFeatureToVehicleAsync(int vehicleId, int featureId)
        {
            if (!await _db.VehicleFeatures
                  .AnyAsync(vf => vf.VehicleId == vehicleId && vf.FeatureId == featureId))
            {
                _db.VehicleFeatures.Add(new VehicleFeature
                {
                    VehicleId = vehicleId,
                    FeatureId = featureId
                });
                await _db.SaveChangesAsync();
            }
        }

        public async Task RemoveFeatureFromVehicleAsync(int vehicleId, int featureId)
        {
            var vf = await _db.VehicleFeatures
                .FirstOrDefaultAsync(x => x.VehicleId == vehicleId && x.FeatureId == featureId);
            if (vf != null)
            {
                _db.VehicleFeatures.Remove(vf);
                await _db.SaveChangesAsync();
            }
        }
    }
}
