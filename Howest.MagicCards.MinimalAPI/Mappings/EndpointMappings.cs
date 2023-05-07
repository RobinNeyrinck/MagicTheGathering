using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Howest.MagicCards.MinimalAPI.Mappings;

public static class EndpointMappings
{
    public static void MapEndpoints(this WebApplication app, string urlPrefix)
    {
        app.MapGet($"{urlPrefix}/cards", (ICardDeckService service) =>
        {
            return service.GetCards();
        })
            .WithTags("CardDeck");

        app.MapGet($"{urlPrefix}/card", (ICardDeckService service, [FromQuery] string name) =>
        {
            return service.GetCard(name);
        })
            .WithTags("CardDeck");

        app.MapPost($"{urlPrefix}/cards", (ICardDeckService service, Card card) =>
        {
            if (card is not null)
            {
                Card createdCard = service.AddCard(card);
                return Results.Created($"{urlPrefix}/cards/{createdCard.Id}", createdCard);
            }
            return Results.BadRequest();
        })
            .WithTags("CardDeck")
            .Accepts<Card>("application/json");

        app.MapPut($"{urlPrefix}/cards", (ICardDeckService service, Card card) =>
        {
            if (card is not null)
            {
                service.UpdateCard(card.Id, card);
                return Results.Ok();
            }
            return Results.BadRequest();
        })
            .WithTags("CardDeck")
            .Accepts<Card>("application/json");

        app.MapDelete($"{urlPrefix}/cards", (ICardDeckService service, string id) =>
        {
            service.RemoveCard(id);
            return Results.Ok();
        })
            .WithTags("CardDeck");
    }
}
