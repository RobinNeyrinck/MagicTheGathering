namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlCardPropertiesRepository : ICardPropertiesRepository
    {
        private readonly mtg_v1Context _db;
        public SqlCardPropertiesRepository(mtg_v1Context context)
        {
            _db = context;
        }
        public IQueryable<Color> GetColors()
        {
            return _db.Colors;
        }
    }
}
