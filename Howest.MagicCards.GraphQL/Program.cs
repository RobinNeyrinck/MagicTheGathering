using GraphQL.Server;
using GraphQL.Server.Ui.Playground;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

builder.Services.AddDbContext<mtg_v1Context>(options =>
    options.UseSqlServer(config.GetConnectionString("mtgDb")));

builder.Services.AddScoped<ICardRepository, SqlCardRepository>();
builder.Services.AddScoped<IArtistRepository, SqlArtistRepository>();

builder.Services.AddScoped<RootSchema>();
builder.Services.AddGraphQL()
    .AddGraphTypes(typeof(RootSchema))
    .AddDataLoader()
    .AddSystemTextJson();

var app = builder.Build();

app.UseGraphQL<RootSchema>();
app.UseGraphQLPlayground(options: new GraphQLPlaygroundOptions()
{
    EditorTheme = EditorTheme.Light
}
);

app.Run();
