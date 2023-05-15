const string commonPrefix = "/api";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
string urlPrefix = builder.Configuration["UrlPrefix"] ?? commonPrefix;

#region MongoDB
builder.Services.Configure<CardDeckDatabaseSettings>(
    builder.Configuration.GetSection(nameof(CardDeckDatabaseSettings))
);

builder.Services.AddSingleton<ICardDeckDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<CardDeckDatabaseSettings>>().Value
);

builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
    new MongoClient(sp.GetRequiredService<ICardDeckDatabaseSettings>().ConnectionString)
);

builder.Services.AddScoped<ICardDeckService, CardDeckService>();
#endregion

builder.Services.AddFluentValidation(v =>
{
	v.RegisterValidatorsFromAssemblyContaining<CardCustomValidator>();
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEndpoints(urlPrefix);

app.Run();
