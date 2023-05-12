namespace Howest.MagicCards.Shared.Extensions;

public static class CardExtensions
{
	public static IQueryable<Card> Sort(this IQueryable<Card> cards, string orderByQueryString)
	{
		if (string.IsNullOrEmpty(orderByQueryString))
		{
			return cards.OrderBy(c => c.Id);
		}

		string[] orderParams = orderByQueryString.Trim().Split(' ');
		if (orderParams.Length != 2 || !orderParams[0].Equals("Name", StringComparison.InvariantCultureIgnoreCase)
			|| (orderParams[1].ToLowerInvariant() != "asc" && orderParams[1].ToLowerInvariant() != "desc"))
		{
			return cards.OrderBy(c => c.Id);
		}

		string propName = orderParams[0];
		bool descending = orderParams[1].ToLowerInvariant() == "desc";

		return descending ? cards.OrderByDescending(c => c.Name) : cards.OrderBy(c => c.Name);
	}




	public static IQueryable<Card> ToFilteredList(this IQueryable<Card> cards, CardFilter filter)
	{
		GetFilters(filter, out string name, out string type, out string cardSet, out string rarity, out string artist,
			out ICollection<string> color, out string power, out string manaCost, out string text);

		return cards
			.Where(c => name == null || c.Name.Contains(name))
			.Where(c => type == null || c.Type.Contains(type))
			.Where(c => cardSet == null || c.SetCodeNavigation.Name == cardSet)
			.Where(c => rarity == null || c.RarityCodeNavigation.Name == rarity)
			.Where(c => artist == null || c.Artist.FullName == artist)
			.Where(c => color == null || color.Count == 0 || c.CardColors.All(cc => color.Contains(cc.Color.Name)))
			.Where(c => power == null || c.Power == power)
			.Where(c => manaCost == null || c.ConvertedManaCost == manaCost)
			.Where(c => text == null || c.Text.Contains(text));
	}


	#region Help Functions
	private static void GetFilters(CardFilter filter, out string name, out string type, out string cardSet, out string rarity,
		out string artist, out ICollection<string> color, out string power, out string manaCost, out string text)
	{
		name = filter.Name;
		type = filter.Type;
		cardSet = filter.CardSet;
		rarity = filter.Rarity;
		artist = filter.Artist;
		color = filter.Color;
		power = filter.Power;
		manaCost = filter.ManaCost;
		text = filter.Text;
	}
	#endregion
}
