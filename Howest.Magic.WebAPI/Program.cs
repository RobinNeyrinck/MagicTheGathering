using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Shared.Mappings;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1.1", new OpenApiInfo
    {
        Title = "MagicCards.WebAPI v1.1",
        Version = "v1.1",
        Description = "MagicCards.WebAPI v1.1",
    });
    c.SwaggerDoc("v1.5", new OpenApiInfo
    {
        Title = "MagicCards.WebAPI v1.5",
        Version = "v1.5",
        Description = "MagicCards.WebAPI v1.5",
    });
});

builder.Services.AddDbContext<mtg_v1Context>(
    options => options.UseSqlServer(config.GetConnectionString("mtgDb"))
);
builder.Services.AddScoped<IArtistRepository, SqlArtistRepository>();
builder.Services.AddScoped<ICardRepository, SqlCardRepository>();
builder.Services.AddScoped<ICardPropertiesRepository, SqlCardPropertiesRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = config.GetConnectionString("redis");
    options.InstanceName = "MagicCards";
});

builder.Services.AddApiVersioning(o =>
{
    o.ReportApiVersions = true;
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 1);
});

builder.Services.AddVersionedApiExplorer(
    options =>
    {
        // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
        // note: the specified format code will format the version as "'v'major[.minor][-status]"
        options.GroupNameFormat = "'v'VVV";

        // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
        // can also be used to control the format of the API version in route templates
        options.SubstituteApiVersionInUrl = true;
    }
);

builder.Services.AddAutoMapper(new System.Type[] {
    typeof(CardProfile),
    typeof(ColorProfile),
    typeof(RarityProfile),
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1.1/swagger.json", "Howest.MagicCards.WebAPI v1.1");
        c.SwaggerEndpoint("/swagger/v1.5/swagger.json", "Howest.MagicCards.WebAPI v1.5");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
