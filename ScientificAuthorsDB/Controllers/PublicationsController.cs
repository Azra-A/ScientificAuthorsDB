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

        // GET: Publications
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

        // GET: Publications/Details/5
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

        // GET: Publications/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Authors = new MultiSelectList(await _context.Authors.ToListAsync(), "Id", "FullName");
            ViewBag.Fields = new MultiSelectList(await _context.ResearchFields.ToListAsync(), "Id", "Name");
            return View();
        }

        // POST: Publications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Publication publication, int[] selectedAuthors, int[] selectedFields, IFormFile? uploadedFile)
        {
            if (ModelState.IsValid)
            {
                // 1. Логика за запазване на файла
                if (uploadedFile != null && uploadedFile.Length > 0)
                {
                    // Създаваме папка wwwroot/uploads, ако не съществува
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Генерираме уникално име, за да не се презапишат файлове с еднакви имена
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(uploadedFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Копираме файла физически в папката
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }

                    // Записваме пътя в модела, който ще се запише в базата данни
                    publication.FilePath = "/uploads/" + uniqueFileName;
                }

                // 2. Съществуващата логика за запазване на публикацията
                _context.Publications.Add(publication);
                await _context.SaveChangesAsync();

                if (selectedAuthors != null)
                {
                    foreach (var authorId in selectedAuthors)
                        _context.AuthorPublications.Add(new AuthorPublication
                        {
                            PublicationId = publication.Id,
                            AuthorId = authorId,
                            ContributionRole = "Съавтор" // <--- Добавяме дефолтна роля
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

        // GET: Publications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var publication = await _context.Publications
                .Include(p => p.AuthorPublications)
                .Include(p => p.PublicationFields)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (publication == null) return NotFound();

            // Взимаме ID-тата на вече избраните автори и области
            var currentAuthors = publication.AuthorPublications.Select(ap => ap.AuthorId).ToList();
            var currentFields = publication.PublicationFields.Select(pf => pf.ResearchFieldId).ToList();

            ViewBag.Authors = new MultiSelectList(await _context.Authors.ToListAsync(), "Id", "FullName", currentAuthors);
            ViewBag.Fields = new MultiSelectList(await _context.ResearchFields.ToListAsync(), "Id", "Name", currentFields);

            return View(publication);
        }

        // POST: Publications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Publication publication, int[] selectedAuthors, int[] selectedFields, IFormFile? uploadedFile)
        {
            if (id != publication.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Проверка дали е качен НОВ файл
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

                        // Записваме новия път (старият път автоматично се презаписва в обекта)
                        publication.FilePath = "/uploads/" + uniqueFileName;
                    }
                    // Ако uploadedFile е null, publication.FilePath си остава със стойността 
                    // от скритото поле във формата (т.е. запазваме стария файл)

                    _context.Update(publication);

                    // махат се старите връзки
                    var existingFields = _context.PublicationFields.Where(pf => pf.PublicationId == id);
                    _context.PublicationFields.RemoveRange(existingFields);

                    // добавят се новите връзки
                    if (selectedFields != null)
                    {
                        foreach (var fieldId in selectedFields)
                            _context.PublicationFields.Add(new PublicationField { PublicationId = id, ResearchFieldId = fieldId });
                    }

                    // 1. Взимаме текущите автори от базата (с техните роли)
                    var existingAuthors = _context.AuthorPublications.Where(ap => ap.PublicationId == id).ToList();

                    // 2. Намираме кои автори са премахнати от потребителя и ги трием
                    var authorsToRemove = existingAuthors.Where(ea => selectedAuthors == null || !selectedAuthors.Contains(ea.AuthorId)).ToList();
                    _context.AuthorPublications.RemoveRange(authorsToRemove);

                    // 3. Добавяме САМО новите автори (без да пипаме старите)
                    if (selectedAuthors != null)
                    {
                        foreach (var authorId in selectedAuthors)
                        {
                            // Ако авторът все още не е свързан с публикацията, го свързваме
                            if (!existingAuthors.Any(ea => ea.AuthorId == authorId))
                            {
                                _context.AuthorPublications.Add(new AuthorPublication
                                {
                                    PublicationId = id,
                                    AuthorId = authorId,
                                    ContributionRole = "Съавтор", // Роля по подразбиране за нови
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

        // GET: Publications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var publication = await _context.Publications
                .FirstOrDefaultAsync(m => m.Id == id);

            if (publication == null) return NotFound();

            return View(publication);
        }

        // POST: Publications/Delete/5
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