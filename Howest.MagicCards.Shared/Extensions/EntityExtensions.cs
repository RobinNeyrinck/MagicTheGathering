namespace Howest.MagicCards.Shared.Extensions
{
    public static class EntityExtensions
    {
        public static IQueryable<T> ToPagedList<T>(this IQueryable<T> values, int pageNumber, int pageSize)
        {
            return values.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
