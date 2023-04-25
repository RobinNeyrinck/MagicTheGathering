using Microsoft.EntityFrameworkCore;

namespace Howest.MagicCards.DAL.Repositories;

public class SqlCardRepository : ICardRepository
{
    private readonly mtg_v1Context _db;
    public SqlCardRepository(mtg_v1Context context)
    {
        _db = context;
    }

    public IQueryable<Card> GetCardByAttribute(string attribute, int value, string comparator)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Card> GetCardByColor(string color)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Card> GetCardByFormat(string format)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Card> GetCardByName(string name)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Card> GetCardByRarity(string rarity)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Card> GetCardBySet(string set)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Card> GetCards()
    {
        IQueryable<Card> cards = _db.Cards
                                    .Include(c => c.Artist);
        return cards;
    }

    public IQueryable<Card> GetCardsByArtist(string artistName)
    {
        throw new NotImplementedException();
    }
}
