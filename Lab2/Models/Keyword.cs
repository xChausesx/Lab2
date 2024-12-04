using System.ComponentModel.DataAnnotations;

namespace Lab2.Models
{
    public class Keyword
    {
        public int Id { get; set; }

        [MaxLength(48)]
        public string Word { get; set; }

        public List<Book>? Books { get; set; }
    }
}
