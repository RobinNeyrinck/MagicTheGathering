namespace GraphQL.GraphQLTypes;

public class RootQuery : ObjectGraphType
{
    public RootQuery(ICardRepository cardRepository, ICardPropertiesRepository cardPropertiesRepository)
    {
        Name = "Query";
        #region Card

        Field<ListGraphType<CardType>>(
                       "cards",
                          Description = "Get all cards",
                          arguments: new QueryArguments(
                              new QueryArgument<IntGraphType> { Name = "first" }
                          ),
                          resolve: context =>
                          {
                              int first = context.GetArgument<int>("first");
                              if (first > 0)
                              {
                                  return cardRepository.GetCards().Take(first).ToList();
                              }
                              return cardRepository.GetCards().ToList();
                          }
        );

        #endregion
    }
}
