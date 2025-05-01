// Holds all pages for adding, editing, and deleting vehicles.

using System.Collections.Generic;
using System.Threading.Tasks;
using KMCSCI3110Project.Data;
using KMCSCI3110Project.Data.Repositories;
using KMCSCI3110Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace KMCSCI3110Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VehiclesController : Controller
    {
        private readonly IVehicleRepository _repo;
        private readonly ApplicationDbContext _db;

        public VehiclesController(IVehicleRepository repo, ApplicationDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        private void PopulateDropdowns()
        {
            ViewBag.ClassList = new List<SelectListItem>
            {
                new("Sedan","Sedan"),
                new("Hatchback","Hatchback"),
                new("SUV","SUV"),
                new("Minivan","Minivan"),
                new("Van","Van"),
                new("Truck","Truck")
            };

            ViewBag.SizeList = new List<SelectListItem>
            {
                new("Compact","Compact"),
                new("Mid","Mid"),
                new("Full","Full")
            };

            ViewBag.GearboxList = new List<SelectListItem>
            {
                new("Automatic","Automatic"),
                new("Manual","Manual")
            };
        }

        // GET: /Vehicles
        public async Task<IActionResult> Index()
        {
            var list = await _repo.GetAllAsync();
            return View(list);
        }

        // GET: /Vehicles/Details/5
        public async Task<IActionResult> Details(int id, bool inquirySubmitted = false)
        {
            var vehicle = await _db.Vehicles
                .Include(v => v.VehicleFeatures)
                    .ThenInclude(vf => vf.Feature)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vehicle == null) return NotFound();

            ViewBag.InquirySubmitted = inquirySubmitted;
            return View(vehicle);
        }

        // GET: /Vehicles/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            PopulateDropdowns();

            ViewBag.AssignedFeatures = new List<int>();
            ViewBag.OpenFeatureModal = false;

            // Pass in an empty Vehicle so Model != null
            return View(new Vehicle());
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            // repopulate dropdowns if validation fails
            PopulateDropdowns();
            if (!ModelState.IsValid)
                return View(vehicle);

            await _repo.AddAsync(vehicle);

            ViewBag.OpenFeatureModal = true;
            ViewBag.AssignedFeatures = new List<int>();

            return View(vehicle);
        }

        // GET: /Vehicles/AssignFeature?
        [HttpGet]
        public async Task<IActionResult> AssignFeature(int vehicleId, int featureId)
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
            return Json(new { status = "Ok" });
        }

        // GET: /Vehicles/RemoveFeature?
        [HttpGet]
        public async Task<IActionResult> RemoveFeature(int vehicleId, int featureId)
        {
            var link = await _db.VehicleFeatures
                .FindAsync(vehicleId, featureId);
            if (link != null)
            {
                _db.VehicleFeatures.Remove(link);
                await _db.SaveChangesAsync();
            }
            return Json(new { status = "Ok" });
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            PopulateDropdowns();
            var v = await _db.Vehicles
                .Include(v => v.VehicleFeatures)
                .FirstOrDefaultAsync(v2 => v2.Id == id);
            if (v == null) return NotFound();
            ViewBag.AssignedFeatures = v.VehicleFeatures.Select(vf => vf.FeatureId).ToList();
            return View(v);
        }

        // POST: Vehicles/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehicle vehicle)
        {
            PopulateDropdowns();
            if (id != vehicle.Id || !ModelState.IsValid)
                return View(vehicle);

            _db.Vehicles.Update(vehicle);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Vehicles/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _repo.GetByIdAsync(id);
            if (vehicle == null) return RedirectToAction(nameof(Index));
            return View(vehicle);
        }

        // POST: /Vehicles/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
