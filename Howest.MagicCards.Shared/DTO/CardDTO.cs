namespace Shared.DTO;

public record CardDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Rarity { get; set; }
    public string Set { get; set; }
    public string Description { get; set; }
    public string Flavor { get; set; }
    public string ArtistName { get; set; }
    public string Number { get; set; }
    public string Power { get; set; }
    public string Toughness { get; set; } 
    public ICollection<string> Colors { get; set; }
    public ICollection<string> Types { get; set; }
}
