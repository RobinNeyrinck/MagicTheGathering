using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<mtg_v1Context>(
	options => options.UseSqlServer(config.GetConnectionString("mtgDb"))
);
builder.Services.AddScoped<ICardRepository, SqlCardRepository>();
builder.Services.AddAutoMapper(new System.Type[] {
	typeof(CardProfile),
});
builder.Services.AddScoped<CardService>();

var app = builder.Build();

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
