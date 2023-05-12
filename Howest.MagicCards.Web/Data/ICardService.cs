namespace Howest.MagicCards.Web.Data;

public interface ICardService
{
	Task<IEnumerable<CardDTO>> Filter(CardFilterArgs filter);
	Task<CardDetailDTO> GetCardByIdAsync(long id);
	Task<IEnumerable<CardDTO>> GetCardsAsync();
	Task<IEnumerable<RarityDTO>> GetRaritiesAsync();
	Task<IEnumerable<SetDTO>> GetSetsAsync();
	Task<IEnumerable<TypeDTO>> GetTypesAsync();
	Task<IEnumerable<CardDTO>> Sort(bool ascending);
}