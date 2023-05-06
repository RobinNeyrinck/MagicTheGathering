using static Howest.MagicCards.Web.Data.CardService;

namespace Howest.MagicCards.Web.Pages;

partial class DeckBuilder
{
    private string title = "DeckBuilder";
    private IEnumerable<CardDTO>? cards = null;
    private IEnumerable<SetDTO>? sets = null;
    private IEnumerable<RarityDTO>? rarities = null;
    private IEnumerable<TypeDTO>? types = null;

    protected override async Task OnInitializedAsync()
    {
        cards = await _cardRepository.GetCardsAsync();
        sets = await _cardRepository.GetSetsAsync();
        rarities = await _cardRepository.GetRaritiesAsync();
        types = await _cardRepository.GetTypesAsync();
    }

    protected async void UpdateCards(CardFilterArgs args)
    {
        cards = await _cardRepository.Filter(args);
    }

    protected async void SortCards(bool ascending)
    {
        cards = await _cardRepository.Sort(ascending);
    }
}
