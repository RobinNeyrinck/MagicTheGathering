using Card = Howest.MagicCards.MinimalAPI.Models.Card;

namespace Howest.MagicCards.Web.Data
{
	public interface IDeckService
	{
		void AddCard(Card card);
		Task<IEnumerable<Card>> GetDeckAsync();
		Task<bool> RemoveCard(Card card);
		Task UpdateCard(Card card);
	}
}