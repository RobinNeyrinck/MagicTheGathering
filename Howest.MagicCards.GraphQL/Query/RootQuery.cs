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
                          resolve: context =>
                          {
                              return cardRepository.GetCards().ToList();
                          }
        );

        #endregion
    }
}
