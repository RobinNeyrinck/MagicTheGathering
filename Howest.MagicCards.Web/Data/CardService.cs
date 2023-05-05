using Microsoft.EntityFrameworkCore;

namespace Howest.MagicCards.Web.Data;

public class CardService
{
	private readonly ICardRepository _cardRepository;
	private readonly IMapper _mapper;
	private readonly int _maxAmount;

	public CardService(ICardRepository cardRepository, IMapper mapper, IConfiguration config)
	{
		_cardRepository = cardRepository;
		_mapper = mapper;
		_maxAmount = config.GetValue<int>("maxPageSize");
	}

	public IEnumerable<CardDTO> GetCards()
	{
		return _cardRepository.GetCards()
			.ProjectTo<CardDTO>(_mapper.ConfigurationProvider)
			.OrderBy(c=> c.Id)
			.Take(_maxAmount)
			.ToList();
	}
}
