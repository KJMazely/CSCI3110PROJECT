// RESTful API for Vehicle Features
using System.Threading.Tasks;
using KMCSCI3110Project.Data;
using KMCSCI3110Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KMCSCI3110Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FeatureController : Controller
    {
        private readonly ApplicationDbContext _db;

        public FeatureController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /Feature
        public async Task<IActionResult> Index()
        {
            var list = await _db.Features.ToListAsync();
            return View(list);
        }

        // GET: /Feature/Create
        public IActionResult Create()
        {
            return View(new Feature());
        }

        // GET: /Feature/GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.Features
                .Select(f => new { f.Id, f.Name })
                .ToListAsync();
            return Json(list);
        }

        // POST: /Feature/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Feature feature)
        {
            if (!ModelState.IsValid)
                return View(feature);

            _db.Features.Add(feature);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Feature/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var feature = await _db.Features.FindAsync(id);
            if (feature == null)
                return RedirectToAction(nameof(Index));

            return View(feature);
        }

        // POST: /Feature/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Feature feature)
        {
            if (id != feature.Id)
                return RedirectToAction(nameof(Index));

            if (!ModelState.IsValid)
                return View(feature);

            _db.Features.Update(feature);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Feature/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var feature = await _db.Features.FindAsync(id);
            if (feature == null)
                return RedirectToAction(nameof(Index));

            return View(feature);
        }

        // POST: /Feature/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feature = await _db.Features.FindAsync(id);
            if (feature != null)
            {
                _db.Features.Remove(feature);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
