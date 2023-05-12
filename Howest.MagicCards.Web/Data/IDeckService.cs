namespace Howest.MagicCards.Web.Data;

public interface IDeckService
{
	Task<bool> AddCard(Card card);
	Task<IEnumerable<Card>> GetDeckAsync();
	Task<bool> RemoveCard(Card card);
	Task<bool> ClearDeck();
}