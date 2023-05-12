namespace Howest.MagicCards.DAL.Repositories;

public interface ICardRepository
{
    IQueryable<Card> GetCardById(int id);
    IQueryable<Card> GetCards();
    IQueryable<Card> GetCardsByArtistId(long id);
}