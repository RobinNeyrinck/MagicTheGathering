namespace Howest.MagicCards.Web.Data
{
	public interface IDeckService
	{
		void AddCard(MinimalAPI.Models.Card card);
		Task<IEnumerable<MinimalAPI.Models.Card>> GetDeckAsync();
		Task RemoveCard(MinimalAPI.Models.Card card);
		Task UpdateCard(MinimalAPI.Models.Card card);
	}
}