using BeerEncyclopedia.Application.BeersServices.EditorServices;
using BeerEncyclopedia.Application.BeersServices.SearchServices;
using BeerEncyclopedia.Application.BeersServices.SearchServices.ByIdSearchServices;
using BeerEncyclopedia.Application.BeersServices.SearchServices.ByNamesSearchServices;
using BeerEncyclopedia.Application.BeersServices.SearchServices.QuerySearchServices;
using BeerEncyclopedia.Application.ColorServices;
using BeerEncyclopedia.Application.CountryServices;
using BeerEncyclopedia.Application.ManufacturerServices;
using BeerEncyclopedia.Application.ShopBeerServices;
using BeerEncyclopedia.Application.Specifications;
using BeerEncyclopedia.Application.StyleServices;
using BeerEncyclopedia.Domain;
using BeerEncyclopedia.Infrastructure.Data;
using BeerEncyclopedia.Infrastructure.Data.Repositories;
using BeerEncyclopedia.Infrastructure.Data.Specifications;
using BeerShared.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BeerEncyclopediaPgDbContext>(c => c.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionString")));
builder.Services.AddHttpClient("ShopBeer", c => c.BaseAddress = new Uri("https://localhost:5001/api/ShopBeer/"));
builder.Services.AddHttpClient("ShopBeerBind", c => c.BaseAddress = new Uri("https://localhost:5001/api/ShopBeerBind/"));
builder.Services.AddScoped<BeerEncyclopediaDbContext, BeerEncyclopediaPgDbContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepository<>));
builder.Services.AddScoped<INameSearchSpecificationFactory<Beer>, BeerFullTextSearchSpecPgFactory>();
builder.Services.AddScoped<INameSearchSpecificationFactory<Manufacturer>, ManufacturerLikeSearchPgFactory>();

builder.Services.AddScoped<IBeerEditorService, BeerEditorRepositoryService>();

builder.Services.AddScoped<IBeerSearchByIdService,BeerSearchByIdRepositoryService>();
builder.Services.AddScoped<IBeerSearchByQueryService, BeerSearchByQueryRepositoryService>();
builder.Services.AddScoped<IBeerSearchByNamesService, BeerSearchByNamesRepositoryService>();

builder.Services.AddScoped<IManufacturerSearchService,ManufacturerSearchService>();
builder.Services.AddScoped<IStyleSearchService, StyleSearchService>();

builder.Services.AddScoped<IColorSearchService, ColorSearchRepositoryService>();

builder.Services.AddScoped<ICountrySearchService, CountrySearchRepositoryService>();

builder.Services.AddTransient<IShopBeerService, ShopBeerServiceHttp>(s =>
{
    var client = s.GetRequiredService<IHttpClientFactory>().CreateClient("ShopBeer");
    return new ShopBeerServiceHttp(client);
}
) ;
builder.Services.AddTransient<IShopBeerBinder, ShopBeerBinderHttp>(s =>
{
    var client = s.GetRequiredService<IHttpClientFactory>().CreateClient("ShopBeerBind");
    return new ShopBeerBinderHttp(client);
}
);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

app.Run();
