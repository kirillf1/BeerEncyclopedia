using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Beers;
using BeerEncyclopedia.Application.Helpers;
using System.Net.Http.Json;

namespace BeerEncyclopedia.Application.BeersServices.EditorServices
{
    public class BeerEditorHttpService : IBeerEditorService
    {
        private readonly HttpClient httpClient;

        public BeerEditorHttpService(HttpClient httpClient)
        {
            if (httpClient.BaseAddress is null)
                throw new ArgumentNullException(nameof(httpClient.BaseAddress));
            this.httpClient = httpClient;
        }
        public async Task<Result> AddBeer(BeerDetails beerDetails, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(httpClient.BaseAddress, beerDetails, cancellationToken);
                return HttpHelper.CheckStatusCode(response);
            }
            catch (Exception e)
            {
                return Result.Error(e.Message);
            }
        }

        public async Task<Result> RemoveBeer(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await httpClient.DeleteAsync(httpClient.BaseAddress + $"/{id}", cancellationToken);
                return HttpHelper.CheckStatusCode(response);
            }
            catch (Exception e)
            {
                return Result.Error(e.Message);
            }
        }

        public async Task<Result> UpdateBeer(BeerDetails beerDetails, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync(httpClient.BaseAddress, beerDetails, cancellationToken);
                return HttpHelper.CheckStatusCode(response);
            }
            catch (Exception e)
            {
                return Result.Error(e.Message);
            }
        }
    }
}
