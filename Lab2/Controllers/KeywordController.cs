using Lab2.Context;
using Lab2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Controllers
{
    public class KeywordController(AppDbContext db) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var keywords = await db.Keywords.ToListAsync();

            return View(keywords);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Keyword keyword)
        {
            if (!ModelState.IsValid)
            {
                return View(keyword);
            }

            db.Add(keyword);

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

            var keyword = await db.Keywords.FindAsync(id);

            if (keyword == null)
            {
                return NotFound();
            }

            return View(keyword);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Keyword keyword)
        {
            if (!ModelState.IsValid)
            {
                return View(keyword);
            }

            db.Keywords.Update(keyword);

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

            var keyword = await db.Keywords.FindAsync(id);

            if (keyword == null)
            {
                return NotFound();
            }

            db.Keywords.Remove(keyword);

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
