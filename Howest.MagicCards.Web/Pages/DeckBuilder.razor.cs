namespace Howest.MagicCards.Web.Pages;

partial class DeckBuilder
{
	private IEnumerable<CardDTO>? _cards;
	private IEnumerable<SetDTO>? _sets;
	private IEnumerable<RarityDTO>? _rarities;
	private IEnumerable<TypeDTO>? _types;
	private IEnumerable<MinimalAPI.Models.Card>? _deck;

	protected override async Task OnInitializedAsync()
	{
		await LoadDataAsync();
	}

	protected async Task UpdateCardsAsync(CardService.CardFilterArgs args)
	{
		_cards = await _cardRepository.Filter(args);
		StateHasChanged();
	}

	protected async Task SortCardsAsync(bool ascending)
	{
		_cards = await _cardRepository.Sort(ascending);
		StateHasChanged();
	}

	protected async Task RemoveCardAsync(MinimalAPI.Models.Card card)
	{
		bool result = await _deckRepository.RemoveCard(card);
		if (result)
		{
			await RefreshDeckAsync();
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
			await RefreshDeckAsync();
		}
	}

	protected async Task AddAnotherCardToDeckAsync(MinimalAPI.Models.Card card)
	{
		bool result = await _deckRepository.AddCard(card);
		if (result)
		{
			await RefreshDeckAsync();
		}
	}

	protected async Task ClearDeckAsync()
	{
		bool result = await _deckRepository.ClearDeck();
		if (result)
		{
			await RefreshDeckAsync();
		}
	}

	private async Task LoadDataAsync()
	{
		_cards = await _cardRepository.GetCardsAsync();
		_sets = await _cardRepository.GetSetsAsync();
		_rarities = await _cardRepository.GetRaritiesAsync();
		_types = await _cardRepository.GetTypesAsync();
		await RefreshDeckAsync();
	}

	private async Task RefreshDeckAsync()
	{
		_deck = await _deckRepository.GetDeckAsync();
		StateHasChanged();
	}
}
