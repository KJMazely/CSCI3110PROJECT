// Main Controller that holds all information to view and reserve cars.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KMCSCI3110Project.Data;
using KMCSCI3110Project.Models;
using KMCSCI3110Project.Models.ViewModels;
using KMCSCI3110Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KMCSCI3110Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Landing page
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        // GET: /Home/Statistics
        public async Task<IActionResult> Statistics()
        {
            var totalVehicles = await _context.Vehicles.CountAsync();
            var availableVehicles = await _context.Vehicles.CountAsync(v => v.IsAvailable);
            var unavailableVehicles = totalVehicles - availableVehicles;
            var averageRentalCost = totalVehicles > 0
                ? await _context.Vehicles.AverageAsync(v => v.RentalCostPerDay)
                : 0m;
            var totalReservations = await _context.Reservations.CountAsync();
            var totalRevenue = await _context.Reservations.SumAsync(r => r.TotalCost);
            var totalInquiries = await _context.BusinessInquiries.CountAsync();
            var totalAccounts = await _context.Users.CountAsync();

            var vm = new StatisticsViewModel
            {
                TotalVehicles = totalVehicles,
                AvailableVehicles = availableVehicles,
                UnavailableVehicles = unavailableVehicles,
                AverageRentalCost = decimal.Round(averageRentalCost, 2),
                TotalReservations = totalReservations,
                TotalRevenue = decimal.Round(totalRevenue, 2),
                TotalInquiries = totalInquiries,
                TotalAccounts = totalAccounts    
            };

            return View(vm);
        }


        // GET: /Home/Vehicles
        public async Task<IActionResult> Vehicles(
            string make,
            string model,
            string vehicleClass,
            string size,
            string gearbox,
            int? year,
            decimal? minPrice,
            decimal? maxPrice,
            string sortBy
        )
        {
            // only available vehicles
            var query = _context.Vehicles.Where(v => v.IsAvailable);

            // Filters
            if (!string.IsNullOrEmpty(make))
                query = query.Where(v => v.Make == make);
            if (!string.IsNullOrEmpty(model))
                query = query.Where(v => v.Model == model);
            if (!string.IsNullOrEmpty(vehicleClass))
                query = query.Where(v => v.VehicleClass == vehicleClass);
            if (!string.IsNullOrEmpty(size))
                query = query.Where(v => v.Size == size);
            if (!string.IsNullOrEmpty(gearbox))
                query = query.Where(v => v.Gearbox == gearbox);
            if (year.HasValue)
                query = query.Where(v => v.Year == year.Value);
            if (minPrice.HasValue)
                query = query.Where(v => v.RentalCostPerDay >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(v => v.RentalCostPerDay <= maxPrice.Value);

            // Sorting
            query = sortBy switch
            {
                "Seats" => query.OrderBy(v => v.SeatAmount),
                "MPG" => query.OrderBy(v => v.MPG),
                "CargoSize" => query.OrderBy(v => v.CargoSize),
                _ => query.OrderBy(v => v.RentalCostPerDay),
            };

            // Populate filter dropdowns
            ViewBag.Makes = await _context.Vehicles.Select(v => v.Make).Distinct().ToListAsync();
            ViewBag.Models = await _context.Vehicles.Select(v => v.Model).Distinct().ToListAsync();
            ViewBag.Classes = new[] { "Sedan", "Hatchback", "SUV", "Minivan", "Van", "Truck" };
            ViewBag.Sizes = new[] { "Compact", "Mid", "Full" };
            ViewBag.Gearboxes = new[] { "Automatic", "Manual" };
            ViewBag.Years = await _context.Vehicles
                                              .Select(v => v.Year)
                                              .Distinct()
                                              .OrderBy(y => y)
                                              .ToListAsync();

            // Sort
            ViewBag.SortOptions = new List<SelectListItem>
            {
                new SelectListItem("Price (default)", ""),
                new SelectListItem("Seat Amount",       "Seats"),
                new SelectListItem("Fuel Efficiency",   "MPG"),
                new SelectListItem("Cargo Capacity",    "CargoSize")
            };
            ViewBag.SelectedSort = sortBy ?? string.Empty;

            var vehicles = await query.ToListAsync();
            return View(vehicles);
        }

        // GET: /Home/Details/5
        public async Task<IActionResult> Details(int id, bool inquirySubmitted = false)
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.VehicleFeatures)
                    .ThenInclude(vf => vf.Feature)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vehicle == null) return NotFound();

            ViewBag.InquirySubmitted = inquirySubmitted;
            return View(vehicle);
        }

        // POST: /Home/ContactUs
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(
            int vehicleId,
            string name,
            string email,
            string inquiryText
        )
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null) return NotFound();

            var biz = new BusinessInquiry
            {
                VehicleId = vehicleId,
                Name = name,
                Email = email,
                Inquiry = inquiryText,
                CreatedAt = DateTime.UtcNow
            };

            _context.BusinessInquiries.Add(biz);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details),
                new { id = vehicleId, inquirySubmitted = true });
        }

        // GET: /Home/Reserve/5
        [Authorize]
        public async Task<IActionResult> Reserve(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound();
            return View(vehicle);
        }

        // POST: /Home/ConfirmReservation
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmReservation(int VehicleId, DateTime StartDate, DateTime EndDate)
        {
            var vehicle = _context.Vehicles.Find(VehicleId);
            if (vehicle == null) return NotFound();

            var days = (EndDate - StartDate).Days + 1;
            var total = vehicle.RentalCostPerDay * days;

            return RedirectToAction(nameof(Checkout), new
            {
                id = VehicleId,
                start = StartDate.ToString("yyyy-MM-dd"),
                end = EndDate.ToString("yyyy-MM-dd"),
                total
            });
        }

        // GET: /Home/Checkout
        [Authorize]
        public IActionResult Checkout(int id, string start, string end, decimal total)
        {
            var vm = new CheckoutViewModel
            {
                VehicleId = id,
                StartDate = DateTime.Parse(start),
                EndDate = DateTime.Parse(end),
                TotalCost = total
            };
            return View(vm);
        }

        // POST: /Home/Checkout
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // Mark vehicle unavailable
            var vehicle = await _context.Vehicles.FindAsync(vm.VehicleId);
            if (vehicle == null) return NotFound();

            vehicle.IsAvailable = false;
            _context.Vehicles.Update(vehicle);

            // Save
            var reservation = new Reservation
            {
                VehicleId = vm.VehicleId,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
                TotalCost = vm.TotalCost
            };
            _context.Reservations.Add(reservation);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ReservationConfirmed));
        }

        // GET: /Home/ReservationConfirmed
        [Authorize]
        public IActionResult ReservationConfirmed()
        {
            return View();
        }
    }
}
