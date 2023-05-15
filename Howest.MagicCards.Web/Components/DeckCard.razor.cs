namespace Howest.MagicCards.Web.Components;

partial class DeckCard
{
	[Parameter]
	public MongoCardDTO Card { get; set; }
	[Parameter]
	public EventCallback<MongoCardDTO> OnNameClicked { get; set; }
	[Parameter]
	public EventCallback<MongoCardDTO> OnNumberClicked { get; set; }

	private async void RemoveCard(MongoCardDTO card)
	{
		await OnNameClicked.InvokeAsync(card);
	}

	private async void AddAnother(MongoCardDTO card)
	{
		await OnNumberClicked.InvokeAsync(card);
	}
}
