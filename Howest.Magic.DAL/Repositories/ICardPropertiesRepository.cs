namespace Howest.MagicCards.DAL.Repositories
{
    public interface ICardPropertiesRepository
    {
        IQueryable<Color> GetColors();
        IQueryable<Rarity> GetRarities();
        IQueryable<Set> GetSets();
    }
}