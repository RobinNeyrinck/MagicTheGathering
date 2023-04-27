using Microsoft.EntityFrameworkCore;

namespace Howest.MagicCards.DAL.Repositories;

public class SqlArtistRepository : IArtistRepository
{
    private readonly mtg_v1Context _db;
    public SqlArtistRepository(mtg_v1Context context)
    {
        _db = context;
    }
    public async Task<Artist> GetArtistAsync(long id)
    {
        return await _db.Artists.FirstOrDefaultAsync(a => a.Id == id);
    }

    public Artist GetArtist(long id)
    {
        return _db.Artists.FirstOrDefault(a => a.Id == id);
    }
}
