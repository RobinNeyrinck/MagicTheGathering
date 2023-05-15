using System.ComponentModel.DataAnnotations;

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

        app.MapPost($"{urlPrefix}/cards", async (ICardDeckService service, MongoCardDTO card, CardCustomValidator validator) =>
        {
			if (card is not null)
            {
				Result.ValidationResult restult = await validator.ValidateAsync(card);
				if (!restult.IsValid)
				{
					return Results.BadRequest(restult.Errors);
				}
				MongoCardDTO createdCard = service.AddCard(card);
                return Results.Created($"{urlPrefix}/cards/{createdCard.Id}", createdCard);
            }
            return Results.BadRequest();
        })
            .WithTags("CardDeck")
            .Accepts<MongoCardDTO>("application/json");

        app.MapPut($"{urlPrefix}/cards", async (ICardDeckService service, MongoCardDTO card, CardCustomValidator validator) =>
        {
            if (card is not null)
            {
                Result.ValidationResult restult = await validator.ValidateAsync(card);
                if (!restult.IsValid)
                {
					return Results.BadRequest(restult.Errors);
				}
                service.UpdateCard(card.Id, card);
                return Results.Ok();
            }
            return Results.BadRequest();
        })
            .WithTags("CardDeck")
            .Accepts<MongoCardDTO>("application/json");

        app.MapDelete($"{urlPrefix}/cards", (ICardDeckService service, string id) =>
        {
            service.RemoveCard(id);
            return Results.Ok();
        })
            .WithTags("CardDeck");

        app.MapDelete($"{urlPrefix}/all", (ICardDeckService service) =>
        {
            service.RemoveAll();
            return Results.Ok();
        })
            .WithTags("CardDeck");
    }
}
