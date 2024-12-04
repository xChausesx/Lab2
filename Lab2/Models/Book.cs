using Lab2.Context;
using Lab2.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab2.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Title { get; set; }

        [Required]
        public int Pages { get; set; }

        public byte[]? File { get; set; }

        public List<Author>? Authors { get; set; }

        public List<Keyword>? Keywords { get; set; }
        
        [NotMapped]
        required public int[] SelectedAuthors { get; set; }
        
        [NotMapped]
        required public int[] SelectedKeywords { get; set; }

        private IFormFile? _formFile;

        [NotMapped]
        public IFormFile? FormFile
        {
            get => _formFile;
            set
            {
                File = FileService.ConvertToByteArr(value);
                _formFile = value;
            }
        }

        public void Update(Book book)
        {
            Title = book.Title;
            Pages = book.Pages;
            Authors = book.Authors;
            Keywords = book.Keywords;

            if (book.FormFile != null)
            {
                FormFile = book.FormFile;
            }
        }
        public async Task InitializeAsync(AppDbContext appDbContext)
        {
            Authors = await appDbContext.Authors.Where(a => SelectedAuthors.Contains(a.Id)).ToListAsync();
            Keywords = await appDbContext.Keywords.Where(a => SelectedKeywords.Contains(a.Id)).ToListAsync();
        }
    }
}
