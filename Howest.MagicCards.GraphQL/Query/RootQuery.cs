namespace GraphQL.GraphQLTypes;

public class RootQuery : ObjectGraphType
{
    public RootQuery(ICardRepository cardRepository)
    {
        Name = "Query";
        #region Card

        Field<ListGraphType<CardType>>(
                       "cards",
                        arguments: new QueryArguments(
                        new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name", Description = "The name of the card." },
                        new QueryArgument<StringGraphType> { Name = "artist", Description = "The name of the artist." },
                        new QueryArgument<StringGraphType> { Name = "color", Description = "The color of the card." },
                        new QueryArgument<StringGraphType> { Name = "type", Description = "The type of the card." },
                        new QueryArgument<StringGraphType> { Name = "rarity", Description = "The rarity of the card." },
                        new QueryArgument<StringGraphType> { Name = "set", Description = "The set of the card." },
                        new QueryArgument<StringGraphType> { Name = "language", Description = "The language of the card." },
                        new QueryArgument<StringGraphType> { Name = "flavor", Description = "The flavor of the card." },
                        new QueryArgument<StringGraphType> { Name = "text", Description = "The text of the card." },
                        new QueryArgument<StringGraphType> { Name = "power", Description = "The power of the card." },
                        new QueryArgument<StringGraphType> { Name = "toughness", Description = "The toughness of the card." },
                        new QueryArgument<StringGraphType> { Name = "loyalty", Description = "The loyalty of the card." },
                        new QueryArgument<StringGraphType> { Name = "watermark", Description = "The watermark of the card." },
                        new QueryArgument<StringGraphType> { Name = "border", Description = "The border of the card." },
                        new QueryArgument<StringGraphType> { Name = "frame", Description = "The frame of the card." },
                        new QueryArgument<StringGraphType> { Name = "frameEffect", Description = "The frame effect of the card." },
                        new QueryArgument<StringGraphType> { Name = "fullArt", Description = "The full art of the card." },
                        new QueryArgument<StringGraphType> { Name = "textless", Description = "The textless of the card." },
                        new QueryArgument<StringGraphType> { Name = "hand", Description = "The hand of the card." },
                        );

        #endregion
    }
}
