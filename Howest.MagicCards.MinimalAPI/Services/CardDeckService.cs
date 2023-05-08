namespace Howest.MagicCards.MinimalAPI.Services;

public class CardDeckService : ICardDeckService
{
    private readonly IMongoCollection<Card> _cardDeck;

    public CardDeckService(ICardDeckDatabaseSettings settings, IMongoClient client)
    {
        IMongoDatabase database = client.GetDatabase(settings.DatabaseName);
        _cardDeck = database.GetCollection<Card>(settings.CardDeckCollectionName);
    }

    public Card AddCard(Card card)
    {
        _cardDeck.InsertOne(card);
        return card;
    }

	public Card GetCard(string name)
	{
        return _cardDeck.Find(card => card.Name == name).FirstOrDefault();
	}

	public List<Card> GetCards()
    {
        return _cardDeck.Find(card => true).ToList();
    }

    public void RemoveCard(string id)
    {
        _cardDeck.DeleteOne(card => card.Id == id);
    }

    public void UpdateCard(string id, Card card)
    {
        _cardDeck.ReplaceOne(card => card.Id == id, card);
    }

    public void RemoveAll()
    {
        _cardDeck.DeleteMany(card => true);
    }
}
