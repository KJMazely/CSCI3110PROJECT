using KMCSCI3110Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KMCSCI3110Project.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<BusinessInquiry> BusinessInquiries { get; set; }

        public DbSet<Feature> Features { get; set; }
        public DbSet<VehicleFeature> VehicleFeatures { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<VehicleFeature>()
              .HasKey(vf => new { vf.VehicleId, vf.FeatureId });

            builder.Entity<VehicleFeature>()
              .HasOne(vf => vf.Vehicle)
              .WithMany(v => v.VehicleFeatures)
              .HasForeignKey(vf => vf.VehicleId);

            builder.Entity<VehicleFeature>()
              .HasOne(vf => vf.Feature)
              .WithMany(f => f.VehicleFeatures)
              .HasForeignKey(vf => vf.FeatureId);
        }
    }
}
