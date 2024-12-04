using Lab2.Services;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab2.Models
{
    public class Author
    {
        public int Id { get; set; }

        public string? FullName { get; set; }

        public string? ShortName { get; set; }

        public string? Bio { get; set; }

        public byte[]? Photo { get; set; }

        public List<Book>? Books { get; set; }

        private IFormFile? _formFile;

        [NotMapped]
        public IFormFile? PhotoFile
        {
            get => _formFile;
            set
            {
                Photo = FileService.ConvertToByteArr(value);
                _formFile = value;
            }
        }

        public void Update(Author author)
        {
            FullName = author.FullName;
            ShortName = author.ShortName;
            Bio = author.Bio;

            if (author.PhotoFile != null)
            {
                PhotoFile = author.PhotoFile;
            }
        }
    }
}