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
    c.SwaggerDoc("1.1", new OpenApiInfo
    {
        Title = "MagicCards.WebAPI 1.1",
        Version = "1.1",
        Description = "MagicCards.WebAPI 1.1",
    });
    c.SwaggerDoc("1.5", new OpenApiInfo
    {
        Title = "MagicCards.WebAPI 1.5",
        Version = "1.5",
        Description = "MagicCards.WebAPI 1.5",
    });
});



builder.Services.AddDbContext<mtg_v1Context>(
    options => options.UseSqlServer(config.GetConnectionString("mtgDb"))
);
builder.Services.AddScoped<IArtistRepository, SqlArtistRepository>();
builder.Services.AddScoped<ICardRepository, SqlCardRepository>();

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
        options.GroupNameFormat = "VVV";

        // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
        // can also be used to control the format of the API version in route templates
        options.SubstituteApiVersionInUrl = true;
    }
);

builder.Services.AddAutoMapper(new System.Type[] { typeof(CardProfile) });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/1.1/swagger.json", "Howest.MagicCards.WebAPI 1.1");
        c.SwaggerEndpoint("/swagger/1.5/swagger.json", "Howest.MagicCards.WebAPI 1.5");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
