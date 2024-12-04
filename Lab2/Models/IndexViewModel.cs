using Lab2.Enums;

namespace Lab2.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Book> Books { get; set; }

        public PageViewModel PageViewModel { get; }
        public FilterViewModel FilterViewModel { get; }
        public SortViewModel SortViewModel { get; } 

        public IndexViewModel(IEnumerable<Book> books, PageViewModel viewModel, FilterViewModel filterViewModel, SortViewModel sortViewModel)
        {
            Books = books;
            PageViewModel = viewModel;
            FilterViewModel = filterViewModel;
            SortViewModel = sortViewModel;
        }
    }
}
