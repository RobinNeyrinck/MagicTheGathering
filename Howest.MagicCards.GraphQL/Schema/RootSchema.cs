using Howest.MagicCards.GraphQL.Query;

namespace Howest.MagicCards.GraphQL.Schema;

public class RootSchema : Schema
{
	public RootSchema(IServiceProvider provider) : base(provider)
	{
		Query = provider.GetRequiredService<RootQuery>();
	}
}
