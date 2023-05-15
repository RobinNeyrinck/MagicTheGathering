namespace Howest.MagicCards.Web.Components;

partial class Card
{
	[Parameter]
	public CardDTO Data { get; set; }
	[Parameter]
	public EventCallback<CardDTO> OnCardClicked { get; set; }

	private void ShowDetailedCard(long cardId)
	{
		navigationManager.NavigateTo($"/cards/{cardId}");
	}

	private async void AddCardToDeck(CardDTO card)
	{
		await OnCardClicked.InvokeAsync(card);
	}

}
