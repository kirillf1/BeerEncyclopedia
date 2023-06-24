using BeerEncyclopedia.Application.BeersServices.EditorServices;
using BeerEncyclopedia.Application.BeersServices.SearchServices;
using BeerEncyclopedia.Application.BeersServices.SearchServices.ByIdSearchServices;
using BeerEncyclopedia.Application.BeersServices.SearchServices.ByImageSearchServices;
using BeerEncyclopedia.Application.BeersServices.SearchServices.ByNamesSearchServices;
using BeerEncyclopedia.Application.BeersServices.SearchServices.QuerySearchServices;
using BeerEncyclopedia.Application.ColorServices;
using BeerEncyclopedia.Application.CountryServices;
using BeerEncyclopedia.Application.ImageSearchServices;
using BeerEncyclopedia.Application.ManufacturerServices;
using BeerEncyclopedia.Application.StyleServices;
using MudBlazor.Services;

namespace BeerEncyclopedia.UI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif
            builder.Services.AddMudServices();
            builder.Services.AddHttpClient("Beers", c => c.BaseAddress = new Uri("https://localhost:5001/api/Beers/query/"));
            builder.Services.AddHttpClient<IBeerSearchByNamesService, BeerSearchByNamesHttpService>(c =>
            c.BaseAddress = new("https://localhost:5001/api/Beers/names"));
            builder.Services.AddHttpClient<IBeerSearchByIdService, BeerSearchByIdHttpService>(c =>
        c.BaseAddress = new("https://localhost:5001/api/Beers/"));
            builder.Services.AddHttpClient<IBeerEditorService, BeerEditorHttpService>(c =>
        c.BaseAddress = new("https://localhost:5001/api/Beers/"));

            builder.Services.AddHttpClient<ICountrySearchService, CountrySearchHttpService>(c =>
           c.BaseAddress = new("https://localhost:5001/api/Countries"));
            builder.Services.AddHttpClient<IColorSearchService, ColorSearchHttpService>(c =>
         c.BaseAddress = new("https://localhost:5001/api/Colors"));
            builder.Services.AddScoped<IBeerSearchByImageService, BeerSearchByImageService>();
            builder.Services.AddScoped<IBeerSearchByQueryService>(s =>
            {
                var client = s.GetRequiredService<IHttpClientFactory>().CreateClient("Beers");
                return new BeerSearchByQueryHttpService(client);
            });
            builder.Services.AddHttpClient("Manufacturers", c => c.BaseAddress = new Uri("https://localhost:5001/api/Manufacturers/"));
            
            builder.Services.AddTransient<IManufacturerSearchService, ManufacuterSearchHttpService>(s =>
            {
                var client = s.GetRequiredService<IHttpClientFactory>().CreateClient("Manufacturers");
                return new ManufacuterSearchHttpService(client);
            });
            
            builder.Services.AddHttpClient<IImageSearchSevice, YandexNameByImageSearchService>(c =>
            {
                c.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
                c.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
                c.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"104\", \" Not A;Brand\";v=\"99\", \"Google Chrome\";v=\"104\"");
                c.DefaultRequestHeaders.Add("accept-language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            });
            builder.Services.AddHttpClient("Styles", c => c.BaseAddress = new Uri("https://localhost:5001/api/Styles/"));
            builder.Services.AddTransient<IStyleSearchService, StyleSearchHttpService>(s =>
            {
                var client = s.GetRequiredService<IHttpClientFactory>().CreateClient("Styles");
                return new StyleSearchHttpService(client);
            });
            return builder.Build();
        }
    }
}