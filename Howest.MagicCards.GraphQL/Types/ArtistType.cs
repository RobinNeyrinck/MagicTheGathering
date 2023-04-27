namespace GraphQL.GraphQLTypes;

public class ArtistType : ObjectGraphType<Artist>
{
    public ArtistType() 
    {
        Name = "Artist";
        Field(a => a.FullName , type: typeof(StringGraphType)).Description("The full name of the artist.");
    }
}