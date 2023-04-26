using Microsoft.EntityFrameworkCore;

namespace Howest.MagicCards.DAL.Repositories;

public class SqlCardRepository : ICardRepository
{
    private readonly mtg_v1Context _db;
    public SqlCardRepository(mtg_v1Context context)
    {
        _db = context;
    }

    public IQueryable<Card> GetCards()
    {
        IQueryable<Card> cards = _db.Cards
                                    .Include(c => c.Artist);
        return cards;
    }
}
