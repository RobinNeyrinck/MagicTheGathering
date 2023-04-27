namespace Howest.MagicCards.MinimalAPI.Models;

public interface ICardDeckDatabaseSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string CardDeckCollectionName { get; set; }
}
