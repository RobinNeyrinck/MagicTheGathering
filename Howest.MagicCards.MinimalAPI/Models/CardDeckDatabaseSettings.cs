namespace Howest.MagicCards.MinimalAPI.Models;

public class CardDeckDatabaseSettings : ICardDeckDatabaseSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string CardDeckCollectionName { get; set; } = string.Empty;
}
