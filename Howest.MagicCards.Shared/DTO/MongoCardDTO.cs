namespace Howest.MagicCards.Shared.DTO;

public class MongoCardDTO
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    [BsonElement("name")]
    public string Name { get; set; }
    [BsonElement("amount")]
    public int Amount { get; set; }
}
