namespace Howest.MagicCards.DAL.Repositories;

public class SqlmtgRepository : ImtgRepository
{
    private readonly mtg_v1Context _db;

    public SqlmtgRepository(mtg_v1Context context)
    {
        _db = context;
    }

    public IEnumerable<Artist> GetArtists()
    {
        return _db.Artists;
    }
}
