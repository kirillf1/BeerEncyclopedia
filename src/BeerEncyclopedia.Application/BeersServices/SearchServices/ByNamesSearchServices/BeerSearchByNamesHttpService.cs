using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Beers;
using BeerEncyclopedia.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerEncyclopedia.Application.BeersServices.SearchServices.ByNamesSearchServices
{
    public class BeerSearchByNamesHttpService : IBeerSearchByNamesService
    {
        private readonly HttpClient httpClient;
        public BeerSearchByNamesHttpService(HttpClient httpClient)
        {
            if (httpClient.BaseAddress == null)
                throw new ArgumentNullException(nameof(httpClient.BaseAddress));
            this.httpClient = httpClient;
        }
        public async Task<Result<IEnumerable<BeerLabel>>> SearchBeerLabels(IEnumerable<string> names, int matchCount = 10, CancellationToken cancellationToken = default)
        {
            try
            {
                var validationResult = BeerSearchByNamesHelper.Validate(names, matchCount);
                if (!validationResult.IsSuccess)
                    return Result.Invalid(validationResult.ValidationErrors);
                var url = httpClient.BaseAddress!.ToString() + BuildQuery(names, matchCount);
                return await HttpHelper.GetAsObject<IEnumerable<BeerLabel>>(httpClient, url, cancellationToken);
            }
            catch(Exception e)
            {
                return Result.Error(e.Message);
            }
        }
        private static string BuildQuery(IEnumerable<string> names,int matchCount)
        {
            var builder = new StringBuilder();
            builder.Append($"?count={matchCount}");
            builder.Append("&names=");
            builder.AppendJoin("&names=", names);
            return builder.ToString();
        }
    }
}
