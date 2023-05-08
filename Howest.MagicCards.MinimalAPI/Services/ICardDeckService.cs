namespace Howest.MagicCards.MinimalAPI.Services;

public interface ICardDeckService
{
    Card AddCard(Card card);
    List<Card> GetCards();
    void RemoveCard(string id);
    void UpdateCard(string id, Card card);
    Card GetCard(string name);
    void RemoveAll();

}
