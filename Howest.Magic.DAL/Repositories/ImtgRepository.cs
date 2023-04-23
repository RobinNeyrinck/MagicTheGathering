namespace Howest.MagicCards.DAL.Repositories
{
    public interface ImtgRepository
    {
        IEnumerable<Artist> GetArtists();
    }
}