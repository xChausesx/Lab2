using Lab2.Enums;

namespace Lab2.Models
{
    public class SortViewModel
    {
        public SortState TitleSort { get; set; }
        public SortState PagesSort { get; set; }
        public SortState AuthorSort { get; }
        public SortState KeywordSort { get; }
        public SortState Current { get; set; }
        public bool Up { get; set; }
        public SortViewModel(SortState sortOrder)
        {
            TitleSort = SortState.TitleAsc;
            PagesSort = SortState.PagesAsc;
            AuthorSort = SortState.AuthorAsc;
            KeywordSort = SortState.KeywordAsc;
            Up = true;

            if (sortOrder.ToString().EndsWith("Desc"))
            {
                Up = false;
            }

            switch (sortOrder)
            {
                case SortState.TitleDesc:
                    Current = TitleSort = SortState.TitleAsc;
                    break;
                case SortState.PagesAsc:
                    Current = PagesSort = SortState.PagesDesc;
                    break;
                case SortState.PagesDesc:
                    Current = PagesSort = SortState.PagesAsc;
                    break;
                case SortState.AuthorAsc:
                    Current = AuthorSort = SortState.AuthorDesc;
                    break;
                case SortState.AuthorDesc:
                    Current = AuthorSort = SortState.AuthorAsc;
                    break;
                case SortState.KeywordAsc:
                    Current = KeywordSort = SortState.KeywordDesc;
                    break;
                case SortState.KeywordDesc:
                    Current = KeywordSort = SortState.KeywordAsc;
                    break;
                default:
                    Current = TitleSort = SortState.TitleDesc;
                    break;
            }
        }
    }
}