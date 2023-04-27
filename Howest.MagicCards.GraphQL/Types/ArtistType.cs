namespace GraphQL.GraphQLTypes;

public class ArtistType : ObjectGraphType<Artist>
{
    public ArtistType(ICardRepository cardRepository) 
    {
        Name = "Artist";
        Field(a => a.FullName , type: typeof(StringGraphType)).Description("The full name of the artist.");
        Field<ListGraphType<CardType>>(
            "cards",
            Description = "The cards of the artist.",
            resolve: context =>
            {
                return cardRepository.GetCardsByArtistId(context.Source.Id);
            }
        );
    }
}