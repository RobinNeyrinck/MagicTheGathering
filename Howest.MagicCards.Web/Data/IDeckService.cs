namespace Howest.MagicCards.Web.Data;

public interface IDeckService
{
	Task<bool> AddCard(MongoCardDTO card);
	Task<IEnumerable<MongoCardDTO>> GetDeckAsync();
	Task<bool> RemoveCard(MongoCardDTO card);
	Task<bool> ClearDeck();
}