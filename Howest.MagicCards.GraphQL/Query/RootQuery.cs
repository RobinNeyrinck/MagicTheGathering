﻿namespace Howest.MagicCards.GraphQL.Query;

public class RootQuery : ObjectGraphType
{
	public RootQuery(ICardRepository cardRepository, IArtistRepository artistRepository)
	{
		Name = "Query";
		#region Card

		Field<ListGraphType<Types.CardType>>(
					   "cards",
						  Description = "Get all cards",
						  arguments: new QueryArguments(
							  new QueryArgument<IntGraphType> { Name = "first" },
							  new QueryArgument<CardFilterType> { Name = "filter" }
						  ),
						  resolve: context =>
						  {
							  int first = context.GetArgument<int>("first");
							  CardFilter filter = context.GetArgument<CardFilter>("filter");

							  if (first > 0)
							  {
								  return cardRepository.GetCards().ToFilteredList(filter).Take(first);
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
			arguments: new QueryArguments(
				new QueryArgument<IntGraphType> { Name = "limit" }
			),
			resolve: context =>
			{
				int limit = context.GetArgument<int>("limit");

				return artistRepository.GetLimitedArtists(limit);
			}
		);

		Field<ArtistType>(
			"artist",
			Description = "Get an artist by id",
			arguments: new QueryArguments(
				new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }
			),
			resolve: context =>
			{
				long id = context.GetArgument<long>("id");
				return artistRepository.GetArtist(id);
			}
		);
		#endregion
	}
}
