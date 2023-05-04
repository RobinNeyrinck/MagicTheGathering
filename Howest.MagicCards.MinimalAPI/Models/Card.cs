namespace Howest.MagicCards.MinimalAPI.Models;

public class Card
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    [BsonElement("name")]
    public string Name { get; set; }
    [BsonElement("amount")]
    public int Amount { get; set; }
}
