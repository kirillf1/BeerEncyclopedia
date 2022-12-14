using BeerShared.Interfaces;
using ShopBeerService.Infrastructure;
using ShopBeerService.Services;
using ShopBeerService.Services.SourceBeerServices;
using ShopBeerService.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<IShopBeerService,BeerService>();
builder.Services.AddScoped<IShopBeerBinder,ShopBeerBinder>();
if (!builder.Environment.IsDevelopment())
{
    WorkersServiceExtension.ConfigureWorkers(builder.Services, builder.Configuration);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

app.Run();
