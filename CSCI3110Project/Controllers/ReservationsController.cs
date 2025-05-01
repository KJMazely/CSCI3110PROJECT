// Holds all information for all reservations customers have made

using KMCSCI3110Project.Data;
using KMCSCI3110Project.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class ReservationsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ReservationsController(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: /Reservations
    public async Task<IActionResult> Index()
    {
        var list = await (from r in _context.Reservations
                          join v in _context.Vehicles
                            on r.VehicleId equals v.Id
                          join u in _userManager.Users
                            on r.UserId equals u.Id
                          select new ReservationAdminViewModel
                          {
                              ReservationId = r.Id,
                              CustomerEmail = u.Email,
                              VehicleId = v.Id,
                              VehicleName = $"{v.Year} {v.Make} {v.Model}",
                              StartDate = r.StartDate,
                              EndDate = r.EndDate,
                              TotalCost = r.TotalCost
                          })
                          .OrderByDescending(r => r.StartDate)
                          .ToListAsync();

        return View(list);
    }

    // GET: /Reservations/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var r = await _context.Reservations.FindAsync(id);
        if (r == null) return NotFound();

        var v = await _context.Vehicles.FindAsync(r.VehicleId);
        var u = _userManager.Users.FirstOrDefault(u => u.Id == r.UserId);

        var vm = new ReservationAdminViewModel
        {
            ReservationId = r.Id,
            CustomerEmail = u?.Email,
            VehicleId = v.Id,
            VehicleName = $"{v.Year} {v.Make} {v.Model}",
            StartDate = r.StartDate,
            EndDate = r.EndDate,
            TotalCost = r.TotalCost
        };
        return View(vm);
    }

    // POST: /Reservations/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var r = await _context.Reservations.FindAsync(id);
        if (r != null)
        {
            // Mark car available again
            var v = await _context.Vehicles.FindAsync(r.VehicleId);
            if (v != null)
            {
                v.IsAvailable = true;
                _context.Vehicles.Update(v);
            }

            _context.Reservations.Remove(r);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
