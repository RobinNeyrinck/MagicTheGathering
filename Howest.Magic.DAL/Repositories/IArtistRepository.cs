namespace Howest.MagicCards.DAL.Repositories;

public interface IArtistRepository
{
    Task<Artist> GetArtistAsync(long id);
}
