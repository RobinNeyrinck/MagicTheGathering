namespace GraphQL.GraphQLTypes;

public class RootSchema : Schema
{
    public RootSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<RootQuery>();
    }
}
