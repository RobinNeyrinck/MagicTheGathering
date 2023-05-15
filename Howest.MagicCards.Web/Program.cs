WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<mtg_v1Context>(
	options => options.UseSqlServer(config.GetConnectionString("mtgDb"))
);
builder.Services.AddScoped<ICardRepository, SqlCardRepository>();
builder.Services.AddScoped<ICardPropertiesRepository, SqlCardPropertiesRepository>();
builder.Services.AddAutoMapper(new System.Type[] {
	typeof(CardProfile),
    typeof(TypeProfile),
    typeof(SetProfile),
    typeof(RarityProfile),
});
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<IDeckService, DeckService>();

builder.Services.AddHttpClient("CardsAPI", client =>
{
	client.BaseAddress = new Uri("https://localhost:7103/api/");
});
builder.Services.AddHttpClient<DeckService>("DeckAPI", client =>
{
	client.BaseAddress = new Uri("https://localhost:7061/api/");
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
