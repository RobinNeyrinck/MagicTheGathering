namespace Howest.MagicCards.Web.Pages;

partial class DeckBuilder
{
    private readonly string _title = "DeckBuilder";
    private IEnumerable<CardDTO>? _cards = null;
    private IEnumerable<SetDTO>? _sets = null;
    private IEnumerable<RarityDTO>? _rarities = null;
    private IEnumerable<TypeDTO>? _types = null;
    private IEnumerable<MinimalAPI.Models.Card>? _deck = null;

    protected override async Task OnInitializedAsync()
    {
        _cards = await _cardRepository.GetCardsAsync();
        _sets = await _cardRepository.GetSetsAsync();
        _rarities = await _cardRepository.GetRaritiesAsync();
        _types = await _cardRepository.GetTypesAsync();
        _deck = await _deckRepository.GetDeckAsync();
    }

    protected async void UpdateCards(CardService.CardFilterArgs args)
    {
        _cards = await _cardRepository.Filter(args);
    }

    protected async void SortCards(bool ascending)
    {
        _cards = await _cardRepository.Sort(ascending);
    }

    protected async void RemoveCardAsync(MinimalAPI.Models.Card card)
    {
		bool result = await _deckRepository.RemoveCard(card);
        if (result)
        {
			_deck = await _deckRepository.GetDeckAsync();
		}
	}

    protected async void AddCardToDeckAsync(CardDTO card)
    {
		MinimalAPI.Models.Card deckCard = new()
        {
            Name = card.Name,
            Amount = 1
        };
        
        bool result = await _deckRepository.AddCard(deckCard);
        if (result)
        {
            _deck = await _deckRepository.GetDeckAsync();
        }
	}

    protected async void AddAnotherCardToDeckAsync(MinimalAPI.Models.Card card)
    {
		bool result = await _deckRepository.AddCard(card);
		if (result)
        {
			_deck = await _deckRepository.GetDeckAsync();
		}
	}

    protected async void ClearDeck()
    {
        bool result = await _deckRepository.ClearDeck();
        if (result)
        {
            _deck = await _deckRepository.GetDeckAsync();
        }
    }
}
