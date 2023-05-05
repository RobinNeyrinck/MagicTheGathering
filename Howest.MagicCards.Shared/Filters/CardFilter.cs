namespace Howest.MagicCards.Shared.Filters
{
    public class CardFilter : PaginationFilter
    {
        public string? Query { get; set; }

        #region filters
        public string Name { get; set; }
        public string Type { get; set; }
        public string CardSet { get; set; }
        public string Rarity { get; set; }
        public string Artist { get; set; }
        public ICollection<string> Color { get; set; }
        public string Power { get; set; }
        public string ManaCost { get; set; }
        public string Text { get; set; }
        public string Toughness { get; set; }
        #endregion
    }
}
