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

    public IQueryable<Card> GetCardById(int id)
    {
        IQueryable<Card> cards = _db.Cards
                                    .Include(c => c.Artist)
                                    .Where(c => c.Id == id);
        return cards;
    }

    public IQueryable<Card> GetCardsByPowerAndToughness(string power, string toughness)
    {
        IQueryable<Card> cards = _db.Cards
                                    .Include(c => c.Artist)
                                    .Where(c => c.Power == power && c.Toughness == toughness);
        return cards;
    }

    public IQueryable<Card> GetCardsByArtistId(long artistId)
    {
        IQueryable<Card> cards = _db.Cards
                                    .Include(c => c.Artist)
                                    .Where(c => c.ArtistId == artistId);
        return cards;
    }
}
