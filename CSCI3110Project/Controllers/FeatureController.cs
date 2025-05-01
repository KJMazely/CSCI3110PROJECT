// RESTful API for Vehicle Features

using KMCSCI3110Project.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class FeatureController : Controller
{
    private readonly ApplicationDbContext _db;
    public FeatureController(ApplicationDbContext db) => _db = db;

    // GET: /Feature/GetAll
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _db.Features
            .Select(f => new { f.Id, f.Name })
            .ToListAsync();
        return Json(list);
    }
}
