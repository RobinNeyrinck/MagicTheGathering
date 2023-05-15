namespace Howest.MagicCards.Web.Pages;

partial class CardDetail
{
	private string? _cardTitle;
	private CardDetailDTO? _card;

	[Parameter]
	public long Id { get; set; }
	protected override async Task OnInitializedAsync()
	{
		_card = await _cardRepository.GetCardByIdAsync(Id);
		_cardTitle = _card?.Name;
	}
}
