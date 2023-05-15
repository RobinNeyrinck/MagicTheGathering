namespace Howest.MagicCards.Web.Components;

partial class CardFilter
{
	[Parameter]
	public IEnumerable<SetDTO>? Sets { get; set; }
	[Parameter]
	public IEnumerable<RarityDTO>? Rarities { get; set; }
	[Parameter]
	public IEnumerable<TypeDTO>? Types { get; set; }
	[Parameter]
	public EventCallback<CardFilterArgs> OnFilterChanged { get; set; }
	[Parameter]
	public EventCallback<bool> OnSortChanged { get; set; }
	private bool _isAscending = false;
	private string Name { get; set; }
	private string Text { get; set; }
	private string Set { get; set; }
	private string Rarity { get; set; }
	private string Type { get; set; }

	public async void OnFilter()
	{
		CardFilterArgs filterArgs = new()
		{
			Name = Name,
			Text = Text,
			Set = Set,
			Rarity = Rarity,
			Type = Type
		};

		await OnFilterChanged.InvokeAsync(filterArgs);
	}

	public async void Sort()
	{
		await OnSortChanged.InvokeAsync(_isAscending);
		_isAscending = !_isAscending;
	}
}
