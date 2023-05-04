namespace Howest.MagicCards.Shared.DTO;

public record CardDTO
{
	public long Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string ArtistName { get; set; }
	public string Set { get; set; }
}
