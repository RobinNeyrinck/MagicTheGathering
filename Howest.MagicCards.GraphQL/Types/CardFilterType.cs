namespace Howest.MagicCards.GraphQL.Types;

public class CardFilterType : InputObjectGraphType<CardFilter>
{
	public CardFilterType()
	{
		Name = "CardFilter";
		Field(x => x.Name, nullable: true);
		Field(x => x.Type, nullable: true);
		Field(x => x.CardSet, nullable: true);
		Field(x => x.Rarity, nullable: true);
		Field(x => x.Artist, nullable: true);
		Field(x => x.Color, nullable: true);
		Field(x => x.Power, nullable: true);
		Field(x => x.ManaCost, nullable: true);
		Field(x => x.Text, nullable: true);
		Field(x => x.Toughness, nullable: true);
	}
}
