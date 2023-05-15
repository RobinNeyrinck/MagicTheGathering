namespace Howest.MagicCards.Web.Components;

partial class CardList
{
	[Parameter]
	public IEnumerable<CardDTO>? Cards { get; set; }
	[Parameter]
	public EventCallback<CardDTO> OnCardClicked { get; set; }

	private async void AddCard(CardDTO card)
	{
		await OnCardClicked.InvokeAsync(card);
	}
}
