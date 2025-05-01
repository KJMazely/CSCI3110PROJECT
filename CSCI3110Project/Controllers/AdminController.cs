// Controller for Admin pages that can only be accessed by admins

using KMCSCI3110Project.Data;
using KMCSCI3110Project.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KMCSCI3110Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _users;

        public AdminController(ApplicationDbContext context,
                       UserManager<IdentityUser> users)
        {
            _context = context;
            _users = users;
        }

        // GET: Admin/Admin/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Admin/Reservations
        public async Task<IActionResult> Reservations()
        {
            var list = await (from r in _context.Reservations
                              join v in _context.Vehicles
                                on r.VehicleId equals v.Id
                              join u in _users.Users
                                on r.UserId equals u.Id
                              select new ReservationAdminViewModel
                              {
                                  ReservationId = r.Id,
                                  CustomerEmail = u.Email,
                                  VehicleName = $"{v.Year} {v.Make} {v.Model}",
                                  StartDate = r.StartDate,
                                  EndDate = r.EndDate,
                                  TotalCost = r.TotalCost
                              })
                              .ToListAsync();

            return View(list);
        }

        public async Task<IActionResult> BusinessInquiries()
        {
            var list = await _context.BusinessInquiries
                .Include(b => b.Vehicle)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
            return View(list);
        }

        // POST: /Admin/DeleteInquiry/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteInquiry(int id)
        {
            var inquiry = await _context.BusinessInquiries.FindAsync(id);
            if (inquiry != null)
            {
                _context.BusinessInquiries.Remove(inquiry);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(BusinessInquiries));
        }
    }
}
