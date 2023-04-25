namespace Howest.MagicCards.Shared.Filters
{
    public class CardFilter : PaginationFilter
    {
        public bool ValidFilters => validateFilters();
        public string? Query { get; set; }

        private bool validateFilters()
        {
            throw new NotImplementedException();
        }
    }
}
