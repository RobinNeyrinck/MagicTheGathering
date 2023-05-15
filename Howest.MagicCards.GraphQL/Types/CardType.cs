namespace Howest.MagicCards.GraphQL.Types;

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
		Field(c => c.OriginalImageUrl, type: typeof(StringGraphType)).Description("The image url of the card.").Name("image");
		Field(c => c.ConvertedManaCost, type: typeof(StringGraphType)).Description("The mana cost of the card.").Name("cmc");
		Field(c => c.SetCode, type: typeof(StringGraphType)).Description("The set code of the card.").Name("set");
		Field(c => c.Power, type: typeof(StringGraphType)).Description("The power of the card.");
		Field(c => c.Toughness, type: typeof(StringGraphType)).Description("The toughness of the card.");
		Field(c => c.Type, type: typeof(StringGraphType)).Description("The type of the card.");
		Field(c => c.RarityCode, type: typeof(StringGraphType)).Description("The rarity of the card.").Name("rarity");
		Field(c => c.Text, type: typeof(StringGraphType)).Description("The text of the card.");
		Field(c => c.Flavor, type: typeof(StringGraphType)).Description("The flavor of the card.");
	}
}
