// Controller for Customer that holds reservation Information

using KMCSCI3110Project.Data;
using KMCSCI3110Project.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize]
public class CustomerController : Controller
{
    private readonly ApplicationDbContext _context;
    public CustomerController(ApplicationDbContext context) => _context = context;

    // GET: /Customer
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var list = await (from r in _context.Reservations
                          where r.UserId == userId
                          join v in _context.Vehicles
                            on r.VehicleId equals v.Id
                          select new CustomerReservationViewModel
                          {
                              Id = r.Id,
                              VehicleName = $"{v.Year} {v.Make} {v.Model}",
                              StartDate = r.StartDate,
                              EndDate = r.EndDate,
                              TotalCost = r.TotalCost
                          }).ToListAsync();
        return View(list);
    }

    // GET: /Customer/Cancel/5
    public async Task<IActionResult> Cancel(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var r = await _context.Reservations.FindAsync(id);
        if (r == null || r.UserId != userId) return RedirectToAction(nameof(Index));
        return View(r);
    }


    [HttpPost, ActionName("Cancel"), ValidateAntiForgeryToken]
    [Authorize]
    // POST: /Reservations/Delete/5
    public async Task<IActionResult> CancelConfirmed(int id)
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
