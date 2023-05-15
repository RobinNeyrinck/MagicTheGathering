using Howest.MagicCards.Shared.DTO;

namespace Howest.MagicCards.MinimalAPI.Services;

public interface ICardDeckService
{
    MongoCardDTO AddCard(MongoCardDTO card);
    List<MongoCardDTO> GetCards();
    void RemoveCard(string id);
    void UpdateCard(string id, MongoCardDTO card);
    MongoCardDTO GetCard(string name);
    void RemoveAll();

}
