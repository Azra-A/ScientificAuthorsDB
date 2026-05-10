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
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Authors
        // СЛЕД РЕДАКЦИЯ
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

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Institutions = new MultiSelectList(await _context.Institutions.ToListAsync(), "Id", "Name");
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,OrcidId,BirthYear")] Author author, int[] selectedInstitutions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();

                foreach (var instId in selectedInstitutions)
                {
                    _context.AuthorInstitutions.Add(new AuthorInstitution
                    {
                        AuthorId = author.Id,
                        InstitutionId = instId
                    });
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Institutions = new MultiSelectList(await _context.Institutions.ToListAsync(), "Id", "Name");
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,OrcidId,BirthYear")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.Id))
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
            return View(author);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
