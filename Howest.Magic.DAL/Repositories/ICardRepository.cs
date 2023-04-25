namespace Howest.MagicCards.DAL.Repositories
{
    public interface ICardRepository
    {
        IQueryable<Card> GetCards();
        IQueryable<Card> GetCardsByArtist(string artistName);
        IQueryable<Card> GetCardByName(string name);
        IQueryable<Card> GetCardByRarity(string rarity);
        IQueryable<Card> GetCardByColor(string color);
        IQueryable<Card> GetCardBySet(string set);
        IQueryable<Card> GetCardByAttribute(string attribute, int value, string comparator);
        IQueryable<Card> GetCardByFormat(string format);
    }
}