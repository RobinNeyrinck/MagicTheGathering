using Howest.MagicCards.Shared.Filters;

namespace GraphQL.GraphQLTypes;

public class RootQuery : ObjectGraphType
{
    public RootQuery(ICardRepository cardRepository, IArtistRepository artistRepository)
    {
        Name = "Query";
        #region Card

        Field<ListGraphType<CardType>>(
                       "cards",
                          Description = "Get all cards",
                          arguments: new QueryArguments(
                              new QueryArgument<IntGraphType> { Name = "first" },
                              new QueryArgument<CardFilterType> { Name = "filter" },
                              new QueryArgument<StringGraphType> { Name = "toughness" }
                          ),
                          resolve: context =>
                          {
                              int first = context.GetArgument<int>("first");
                              CardFilter filter = context.GetArgument<CardFilter>("filter");

                              if (first > 0)
                              {
                                  return cardRepository.GetCards().Take(first).ToList();
                              }

                              return cardRepository
                                    .GetCards()
                                    .ToFilteredList(filter);
                          }
        );

        #endregion

        #region Artists
        Field<ListGraphType<ArtistType>>(
            "artists",
            Description = "Get all artists",
            resolve: context => artistRepository.GetArtistsAsync()
        );
        #endregion
    }
}
