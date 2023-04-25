namespace Howest.MagicCards.DAL.Repositories
{
    public interface IArtistRepository
    {
        IQueryable<Artist> GetArtists();
    }
}