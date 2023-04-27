using System.Reflection;
using System.Linq.Dynamic.Core;
using System.Text;
using Howest.MagicCards.Shared.Filters;

namespace Howest.MagicCards.Shared.Extensions;

public static class CardExtensions
{
    public static IQueryable<Card> Sort(this IQueryable<Card> cards, string orderByQueryString)
    {
        // TODO: fix sorting
        if (string.IsNullOrEmpty(orderByQueryString))
        {
            return cards.OrderBy(c => c.Name);
        }

        string[] orderParameters = orderByQueryString.Trim().Split(',');
        PropertyInfo[] propertyInfos = typeof(Card).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        StringBuilder orderQueryBuilder = new StringBuilder();

        foreach (string param in orderParameters)
        {
            if (!string.IsNullOrWhiteSpace(param))
            {
                string propertyFromQueryName = param.Split(" ")[0];
                PropertyInfo objectProperty = propertyInfos
                                 .FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty is not null)
                {
                    string direction = param.EndsWith(" desc") ? "descending" : "ascending";
                    orderQueryBuilder.Append($"{objectProperty.Name} {direction}, ");
                }
            }
        }

        string orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
        if (string.IsNullOrWhiteSpace(orderQuery))
        {
            return cards.OrderBy(c => c.Name);
        }

        return cards.OrderBy(orderQuery);
    }

    public static IQueryable<Card> ToFilteredList(this IQueryable<Card> cards, CardFilter filter)
    {
        GetFilters(filter, out string name, out string type, out string cardSet, out string rarity, out string artist, out ICollection<string> color, out string power, out string manaCost, out string text);

        return cards.
            Where(c => name == null || c.Name.Contains(name)).
            Where(c => type == null || c.Type.Contains(type)).
            Where(c => cardSet == null || c.SetCodeNavigation.Name == cardSet).
            Where(c => rarity == null || c.RarityCodeNavigation.Name == rarity).
            Where(c => artist == null || c.Artist.FullName == artist).
            Where(c => color == null || color.Count == 0 || c.CardColors.All(cc => color.Contains(cc.Color.Name))).
            Where(c => power == null || c.Power == power).
            Where(c => manaCost == null || c.ConvertedManaCost == manaCost).
            Where(c => text == null || c.Text.Contains(text));

    }


    #region Help Functions
    private static void GetFilters(CardFilter filter, out string name, out string type, out string cardSet, out string rarity, out string artist, out ICollection<string> color, out string power, out string manaCost, out string text)
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
