namespace GraphQL.GraphQLTypes;

public class CardType : ObjectGraphType<Card>
{
    public CardType(IArtistRepository artistRepository)
    {
        Name = "Card";
        Field(c => c.Id, type: typeof(IdGraphType)).Description("The ID of the card.");
        Field(c => c.Name, type: typeof(StringGraphType)).Description("The name of the card.");
        Field<ArtistType>(
            "artist",
            resolve: context => artistRepository.GetArtist(context.Source.ArtistId ?? default)
        );
    }
}
