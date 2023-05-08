using static Howest.MagicCards.Web.Data.CardService;

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

    protected async void UpdateCards(CardFilterArgs args)
    {
        _cards = await _cardRepository.Filter(args);
    }

    protected async void SortCards(bool ascending)
    {
        _cards = await _cardRepository.Sort(ascending);
    }

    protected async Task RemoveCardAsync(MinimalAPI.Models.Card card)
    {
		bool result = await _deckRepository.RemoveCard(card);
        if (result)
        {
			_deck = await _deckRepository.GetDeckAsync();
		}
	}

    protected async Task AddCardToDeckAsync(CardDTO card)
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
}
