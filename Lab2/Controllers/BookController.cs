using Lab2.Context;
using Lab2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab2.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Lab2.Controllers
{
    public class BookController(AppDbContext db) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(string? title, int author = 0, int keyword = 0, SortState sortOrder = SortState.TitleAsc, int page = 1, int pageSize = 2)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var books = db.Books.Include(b => b.Authors).Include(b => b.Keywords).AsQueryable();

            if (author != 0)
            {
                books = books.Where(b => b.Authors!.Any(a => a.Id == author));
            }
            if (keyword != 0)
            {
                books = books.Where(b => b.Keywords!.Any(k => k.Id == keyword));
            }
            if (!string.IsNullOrEmpty(title))
            {
                books = books.Where(b => b.Title!.Contains(title));
            }

            var count = await books.CountAsync();

            var items = books.Skip((page - 1) * pageSize).Take(pageSize).AsQueryable();

            items = sortOrder switch
            {
                SortState.TitleDesc => items.OrderByDescending(b => b.Title),
                SortState.PagesAsc => items.OrderBy(b => b.Pages),
                SortState.PagesDesc => items.OrderByDescending(b => b.Pages),
                SortState.AuthorAsc => items.OrderBy(b => b.Authors!.FirstOrDefault()!.FullName),
                SortState.AuthorDesc => items.OrderByDescending(b => b.Authors!.FirstOrDefault()!.FullName),
                SortState.KeywordAsc => items.OrderBy(b => b.Keywords!.FirstOrDefault()!.Word),
                SortState.KeywordDesc => items.OrderByDescending(b => b.Keywords!.FirstOrDefault()!.Word),
                _ => items.OrderBy(b => b.Title),
            };

            IndexViewModel viewModel = new(
            items,
            new PageViewModel(count, page, pageSize),
            new FilterViewModel(await db.Authors.ToListAsync(), await db.Keywords.ToListAsync(), author, keyword, title),
            new SortViewModel(sortOrder)
            );

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFile(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var book = await db.Books.FindAsync(id);

            return File(book!.File!, "application/octet-stream", $"{book.Title}.pdf");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewData["Authors"] = new SelectList(await db.Authors.ToListAsync(), "Id", "FullName");
            ViewData["Keywords"] = new SelectList(await db.Keywords.ToListAsync(), "Id", "Word");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Book book)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Authors"] = new SelectList(await db.Authors.ToListAsync(), "Id", "FullName");
                ViewData["Keywords"] = new SelectList(await db.Keywords.ToListAsync(), "Id", "Word");

                return View(book);
            }

            await book.InitializeAsync(db);

            db.Add(book);

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

            var book = await db.Books.Include(b => b.Authors).Include(b => b.Keywords).FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            ViewData["Authors"] = new SelectList(await db.Authors.ToListAsync(), "Id", "FullName", book.Authors!.Select(a => a.Id));
            ViewData["Keywords"] = new SelectList(await db.Keywords.ToListAsync(), "Id", "Word", book.Keywords!.Select(k => k.Id));

            return View(book);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Book book)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Authors"] = new SelectList(await db.Authors.ToListAsync(), "Id", "FullName", book.Authors!.Select(a => a.Id));
                ViewData["Keywords"] = new SelectList(await db.Keywords.ToListAsync(), "Id", "Word", book.Keywords!.Select(k => k.Id));
                return View(book);
            }

            await book.InitializeAsync(db);

            var existedBook = await db.Books.Include(b => b.Authors).Include(b => b.Keywords).FirstOrDefaultAsync(b => b.Id == book.Id);

            existedBook!.Update(book);

            db.Update(existedBook);

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

            var book = await db.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            db.Books.Remove(book);

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }

}
