namespace Howest.MagicCards.Shared.Extensions
{
	public static class EntityExtensions
	{
		public static IQueryable<T> ToPagedList<T>(this IQueryable<T> values, int pageSize, int skipAmount)
		{
			return values
					.Skip(skipAmount)
					.Take(pageSize);

		}
	}
}
