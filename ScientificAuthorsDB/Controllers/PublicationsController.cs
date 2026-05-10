using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public PublicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Publications
        public async Task<IActionResult> Index(string? searchTitle, string? searchType, int? searchYear)
        {
            ViewData["SearchTitle"] = searchTitle;
            ViewData["SearchType"] = searchType;
            ViewData["SearchYear"] = searchYear;
            ViewBag.Types = new SelectList(new[] { "Article", "Conference", "Book", "Thesis" }, searchType);

            var publications = _context.Publications
                .Include(p => p.AuthorPublications).ThenInclude(ap => ap.Author)
                .Include(p => p.PublicationFields).ThenInclude(pf => pf.ResearchField)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTitle))
                publications = publications.Where(p => p.Title.Contains(searchTitle));

            if (!string.IsNullOrEmpty(searchType))
                publications = publications.Where(p => p.PublicationType == searchType);

            if (searchYear.HasValue)
                publications = publications.Where(p => p.Year == searchYear);

            return View(await publications.ToListAsync());
        }

        // GET: Publications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publication = await _context.Publications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publication == null)
            {
                return NotFound();
            }

            return View(publication);
        }

        // GET: Publications/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Authors = new MultiSelectList(await _context.Authors.ToListAsync(), "Id", "FullName");
            ViewBag.Fields = new MultiSelectList(await _context.ResearchFields.ToListAsync(), "Id", "Name");
            ViewBag.Types = new SelectList(new[] { "Article", "Conference", "Book", "Thesis" });
            return View();
        }

        // POST: Publications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Abstract,Year,Doi,Journal,PublicationType")] Publication publication, int[] selectedAuthors, int[] selectedFields)
        {
            if (ModelState.IsValid)
            {
                _context.Add(publication);
                await _context.SaveChangesAsync();

                foreach (var authorId in selectedAuthors)
                {
                    _context.AuthorPublications.Add(new AuthorPublication
                    {
                        PublicationId = publication.Id,
                        AuthorId = authorId
                    });
                }

                foreach (var fieldId in selectedFields)
                {
                    _context.PublicationFields.Add(new PublicationField
                    {
                        PublicationId = publication.Id,
                        ResearchFieldId = fieldId
                    });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Authors = new MultiSelectList(await _context.Authors.ToListAsync(), "Id", "FullName");
            ViewBag.Fields = new MultiSelectList(await _context.ResearchFields.ToListAsync(), "Id", "Name");
            ViewBag.Types = new SelectList(new[] { "Article", "Conference", "Book", "Thesis" });
            return View(publication);
        }

        // GET: Publications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publication = await _context.Publications.FindAsync(id);
            if (publication == null)
            {
                return NotFound();
            }
            return View(publication);
        }

        // POST: Publications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Abstract,Year,Doi,Journal,PublicationType")] Publication publication)
        {
            if (id != publication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublicationExists(publication.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(publication);
        }

        // GET: Publications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publication = await _context.Publications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publication == null)
            {
                return NotFound();
            }

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
            return RedirectToAction(nameof(Index));
        }

        private bool PublicationExists(int id)
        {
            return _context.Publications.Any(e => e.Id == id);
        }
    }
}
