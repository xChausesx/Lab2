using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.Tracing;

namespace Lab2.Models
{
    public class FilterViewModel
    {
        public FilterViewModel(List<Author> authors, List<Keyword> keywords, int author, int keyword, string? title)
        {
            authors.Insert(0, new Author { FullName = "Всі", Id = 0 });
            keywords.Insert(0, new Keyword { Word = "Всі", Id = 0 });
            Authors = new SelectList(authors, "Id", "FullName", author);
            Keywords = new SelectList(keywords, "Id", "Word", keyword);
            SelectedAuthor = author;
            SelectedKeyword = keyword;
            SelectedTitle = title;
        }
        public SelectList Authors { get; }
        public SelectList Keywords{ get; }
        public int SelectedAuthor { get; }
        public int SelectedKeyword { get; }
        public string? SelectedTitle { get; }
    }
}
