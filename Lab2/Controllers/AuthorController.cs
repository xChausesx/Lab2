using Lab2.Context;
using Lab2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Controllers
{
    public class AuthorController(AppDbContext db) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var authors = await db.Authors.ToListAsync();
            return View(authors);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Author author)
        {
            if (!ModelState.IsValid)
            {
                return View(author);
            }

            db.Add(author);

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var author = await db.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Author author)
        {
            if (!ModelState.IsValid)
            {
                return View(author);
            }

            var existedAuthor = await db.Authors.FindAsync(author.Id);

            existedAuthor!.Update(author);

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var author = await db.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            db.Authors.Remove(author);

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
