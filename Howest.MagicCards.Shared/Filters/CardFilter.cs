namespace Howest.MagicCards.Shared.Filters
{
    public class CardFilter : PaginationFilter
    {
        public bool ValidFilters => validateFilters();
        public string? Query { get; set; }

        #region filters
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? CardSet { get; set; }
        public string? Rarity { get; set; }
        public string? Artist { get; set; }
        public ICollection<string>? Color { get; set; }
        public string? Power { get; set; }
        public string? ManaCost { get; set; }
        public string? Text { get; set; }
        #endregion

        private bool validateFilters()
        {
            throw new NotImplementedException();
        }
    }
}
