using Microsoft.EntityFrameworkCore;

namespace Howest.MagicCards.DAL.Repositories;

public class SqlArtistRepository : IArtistRepository
{
    private readonly mtg_v1Context _db;

    public SqlArtistRepository(mtg_v1Context context)
    {
        _db = context;
    }

    public IQueryable<Artist> GetArtists()
    {
        IQueryable<Artist> artists = _db.Artists;
            // TODO: add cards to artist

        return artists;
    }
}
