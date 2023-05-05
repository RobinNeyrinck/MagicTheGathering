using Microsoft.EntityFrameworkCore;

namespace Howest.MagicCards.Web.Data;

public class CardService
{
	private readonly ICardRepository _cardRepository;
	private readonly ICardPropertiesRepository _cardProperties;
	private readonly IMapper _mapper;
	private readonly int _maxAmount;

	public CardService(ICardRepository cardRepository, IMapper mapper, IConfiguration config, ICardPropertiesRepository cardProperties)
	{
		_cardRepository = cardRepository;
		_cardProperties = cardProperties;
		_mapper = mapper;
		_maxAmount = config.GetValue<int>("maxPageSize");
	}

	public IEnumerable<CardDTO> GetCards()
	{
		return _cardRepository.GetCards()
			.ProjectTo<CardDTO>(_mapper.ConfigurationProvider)
			.OrderBy(c => c.Id)
			.Take(_maxAmount)
			.ToList();
	}

	public IEnumerable<SetDTO> GetSets()
	{
		return _cardProperties.GetSets()
			.ProjectTo<SetDTO>(_mapper.ConfigurationProvider)
			.ToList();
	}

	public IEnumerable<TypeDTO> GetTypes()
	{
		return _cardProperties.GetTypes()
			.ProjectTo<TypeDTO>(_mapper.ConfigurationProvider)
			.ToList();
	}

	public IEnumerable<RarityDTO> GetRarities()
	{
		return _cardProperties.GetRarities()
			.ProjectTo<RarityDTO>(_mapper.ConfigurationProvider)
			.ToList();
	}
}
