using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScientificAuthorsDB.Data;
using ScientificAuthorsDB.Models;

namespace ScientificAuthorsDB.Controllers
{
    public class PublicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublicationsController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index(string? searchTitle, string? searchType)
        {
            ViewData["SearchTitle"] = searchTitle;

            var pubs = _context.Publications
                .Include(p => p.AuthorPublications).ThenInclude(ap => ap.Author)
                .Include(p => p.PublicationFields).ThenInclude(pf => pf.ResearchField)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTitle))
                pubs = pubs.Where(p => p.Title.Contains(searchTitle));

            if (!string.IsNullOrEmpty(searchType))
                pubs = pubs.Where(p => p.PublicationType == searchType);

            return View(await pubs.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var publication = await _context.Publications
                .Include(p => p.AuthorPublications).ThenInclude(ap => ap.Author)
                .Include(p => p.PublicationFields).ThenInclude(pf => pf.ResearchField)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (publication == null) return NotFound();

            return View(publication);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Authors = new MultiSelectList(await _context.Authors.ToListAsync(), "Id", "FullName");
            ViewBag.Fields = new MultiSelectList(await _context.ResearchFields.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Publication publication, int[] selectedAuthors, int[] selectedFields, IFormFile? uploadedFile)
        {
            if (ModelState.IsValid)
            {
                if (uploadedFile != null && uploadedFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(uploadedFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }

                    publication.FilePath = "/uploads/" + uniqueFileName;
                }

                _context.Publications.Add(publication);
                await _context.SaveChangesAsync();

                if (selectedAuthors != null)
                {
                    foreach (var authorId in selectedAuthors)
                        _context.AuthorPublications.Add(new AuthorPublication
                        {
                            PublicationId = publication.Id,
                            AuthorId = authorId,
                            ContributionRole = "Съавтор"
                        });
                }

                if (selectedFields != null)
                {
                    foreach (var fieldId in selectedFields)
                        _context.PublicationFields.Add(new PublicationField { PublicationId = publication.Id, ResearchFieldId = fieldId });
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Публикацията е добавена успешно!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Authors = new MultiSelectList(await _context.Authors.ToListAsync(), "Id", "FullName");
            ViewBag.Fields = new MultiSelectList(await _context.ResearchFields.ToListAsync(), "Id", "Name");
            return View(publication);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var publication = await _context.Publications
                .Include(p => p.AuthorPublications)
                .Include(p => p.PublicationFields)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (publication == null) return NotFound();

            var currentAuthors = publication.AuthorPublications.Select(ap => ap.AuthorId).ToList();
            var currentFields = publication.PublicationFields.Select(pf => pf.ResearchFieldId).ToList();

            ViewBag.Authors = new MultiSelectList(await _context.Authors.ToListAsync(), "Id", "FullName", currentAuthors);
            ViewBag.Fields = new MultiSelectList(await _context.ResearchFields.ToListAsync(), "Id", "Name", currentFields);

            return View(publication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Publication publication, int[] selectedAuthors, int[] selectedFields, IFormFile? uploadedFile)
        {
            if (id != publication.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (uploadedFile != null && uploadedFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(uploadedFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(fileStream);
                        }

                        publication.FilePath = "/uploads/" + uniqueFileName;
                    }

                    _context.Update(publication);


                    var existingFields = _context.PublicationFields.Where(pf => pf.PublicationId == id);
                    _context.PublicationFields.RemoveRange(existingFields);

                    if (selectedFields != null)
                    {
                        foreach (var fieldId in selectedFields)
                            _context.PublicationFields.Add(new PublicationField { PublicationId = id, ResearchFieldId = fieldId });
                    }


                    var existingAuthors = _context.AuthorPublications.Where(ap => ap.PublicationId == id).ToList();

                    var authorsToRemove = existingAuthors.Where(ea => selectedAuthors == null || !selectedAuthors.Contains(ea.AuthorId)).ToList();
                    _context.AuthorPublications.RemoveRange(authorsToRemove);

                    if (selectedAuthors != null)
                    {
                        foreach (var authorId in selectedAuthors)
                        {
                            if (!existingAuthors.Any(ea => ea.AuthorId == authorId))
                            {
                                _context.AuthorPublications.Add(new AuthorPublication
                                {
                                    PublicationId = id,
                                    AuthorId = authorId,
                                    ContributionRole = "Съавтор",
                                    AuthorOrder = 2
                                });
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Публикацията е обновена успешно!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Publications.Any(e => e.Id == publication.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Authors = new MultiSelectList(await _context.Authors.ToListAsync(), "Id", "FullName");
            ViewBag.Fields = new MultiSelectList(await _context.ResearchFields.ToListAsync(), "Id", "Name");
            return View(publication);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var publication = await _context.Publications
                .FirstOrDefaultAsync(m => m.Id == id);

            if (publication == null) return NotFound();

            return View(publication);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publication = await _context.Publications.FindAsync(id);
            if (publication != null)
            {
                _context.Publications.Remove(publication);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Публикацията беше изтрита.";
            return RedirectToAction(nameof(Index));
        }
    }
}