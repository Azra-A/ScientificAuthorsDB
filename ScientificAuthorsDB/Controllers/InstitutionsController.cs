using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScientificAuthorsDB.Data;
using ScientificAuthorsDB.Models;

namespace ScientificAuthorsDB.Controllers
{
    public class InstitutionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InstitutionsController(ApplicationDbContext context) => _context = context;

        // списък с институции + търсене
        public async Task<IActionResult> Index(string? searchName, string? searchCountry)
        {
            ViewData["SearchName"] = searchName;
            ViewData["SearchCountry"] = searchCountry;

            var institutions = _context.Institutions
                .Include(i => i.AuthorInstitutions).ThenInclude(ai => ai.Author)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchName))
                institutions = institutions.Where(i => i.Name.Contains(searchName));

            if (!string.IsNullOrEmpty(searchCountry))
                institutions = institutions.Where(i => i.Country != null && i.Country.Contains(searchCountry));

            return View(await institutions.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var inst = await _context.Institutions
                .Include(i => i.AuthorInstitutions).ThenInclude(ai => ai.Author)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inst == null) return NotFound();
            return View(inst);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Institution institution)
        {
            if (ModelState.IsValid)
            {
                _context.Institutions.Add(institution);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Институцията е добавена успешно!";
                return RedirectToAction(nameof(Index));
            }
            return View(institution);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var inst = await _context.Institutions.FindAsync(id);
            if (inst == null) return NotFound();
            return View(inst);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Institution institution)
        {
            if (id != institution.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(institution);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Институцията е обновена!";
                return RedirectToAction(nameof(Index));
            }
            return View(institution);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var inst = await _context.Institutions.FirstOrDefaultAsync(i => i.Id == id);
            if (inst == null) return NotFound();
            return View(inst);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inst = await _context.Institutions.FindAsync(id);
            if (inst != null) _context.Institutions.Remove(inst);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}