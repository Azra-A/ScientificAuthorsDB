using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScientificAuthorsDB.Data;
using ScientificAuthorsDB.Models;

namespace ScientificAuthorsDB.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthorsController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index(string? searchName, string? searchField, int? searchYear)
        {
            ViewData["SearchName"] = searchName;
            ViewData["SearchField"] = searchField;
            ViewData["SearchYear"] = searchYear;

            ViewBag.Fields = new SelectList(await _context.ResearchFields.ToListAsync(), "Name", "Name", searchField);

            var authors = _context.Authors
                .Include(a => a.AuthorInstitutions).ThenInclude(ai => ai.Institution)
                .Include(a => a.AuthorPublications).ThenInclude(ap => ap.Publication)
                    .ThenInclude(p => p.PublicationFields).ThenInclude(pf => pf.ResearchField)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchName))
                authors = authors.Where(a => a.FirstName.Contains(searchName) || a.LastName.Contains(searchName));

            if (!string.IsNullOrEmpty(searchField))
                authors = authors.Where(a =>
                    a.AuthorPublications.Any(ap =>
                        ap.Publication.PublicationFields.Any(pf => pf.ResearchField.Name == searchField)));

            if (searchYear.HasValue)
                authors = authors.Where(a =>
                    a.AuthorPublications.Any(ap => ap.Publication.Year == searchYear));

            return View(await authors.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var author = await _context.Authors
                .Include(a => a.AuthorInstitutions).ThenInclude(ai => ai.Institution)
                .Include(a => a.AuthorPublications).ThenInclude(ap => ap.Publication)
                    .ThenInclude(p => p.PublicationFields).ThenInclude(pf => pf.ResearchField)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null) return NotFound();

            return View(author);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Institutions = new MultiSelectList(await _context.Institutions.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author, int[] selectedInstitutions)
        {
            if (ModelState.IsValid)
            {
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();

                foreach (var id in selectedInstitutions)
                    _context.AuthorInstitutions.Add(new AuthorInstitution { AuthorId = author.Id, InstitutionId = id });

                await _context.SaveChangesAsync();
                TempData["Success"] = "Авторът е добавен успешно!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Institutions = new MultiSelectList(await _context.Institutions.ToListAsync(), "Id", "Name");
            return View(author);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var author = await _context.Authors
                .Include(a => a.AuthorInstitutions)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null) return NotFound();

            var selected = author.AuthorInstitutions.Select(ai => ai.InstitutionId);
            ViewBag.Institutions = new MultiSelectList(await _context.Institutions.ToListAsync(), "Id", "Name", selected);
            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Author author, int[] selectedInstitutions)
        {
            if (id != author.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(author);

                var existing = _context.AuthorInstitutions.Where(ai => ai.AuthorId == id);
                _context.AuthorInstitutions.RemoveRange(existing);

                foreach (var instId in selectedInstitutions)
                    _context.AuthorInstitutions.Add(new AuthorInstitution { AuthorId = id, InstitutionId = instId });

                await _context.SaveChangesAsync();
                TempData["Success"] = "Авторът е обновен успешно!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Institutions = new MultiSelectList(await _context.Institutions.ToListAsync(), "Id", "Name");
            return View(author);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);
            if (author == null) return NotFound();

            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null) _context.Authors.Remove(author);

            await _context.SaveChangesAsync();
            TempData["Success"] = "Авторът е изтрит.";
            return RedirectToAction(nameof(Index));
        }
    }
}