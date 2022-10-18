using Ardalis.Result;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace BeerEncyclopedia.Application.Helpers
{
    public static class HttpHelper
    {
        /// <summary>
        /// Checks for successful status code if not throws an HttpRequestException
        /// </summary>
        /// <param name="httpStatusCode"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public static bool CheckStatusCode(HttpStatusCode httpStatusCode)
        {
            int statusCode = (int)httpStatusCode;
            if (statusCode > 299)
                throw new HttpRequestException($"Request failed with code {statusCode}");
            return true;
        }
        public static Result CheckStatusCode(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return Result.Success();
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return Result.Unauthorized();
            else if (response.StatusCode == HttpStatusCode.Forbidden)
                return Result.Forbidden();
            else if (response.StatusCode == HttpStatusCode.NotFound)
                return Result.NotFound();
            else
                return Result.Error($"Request failed with code {(int)response.StatusCode}");
        }
        public static async Task<Result<T>> GetAsObject<T>(this HttpClient httpClient, string url, CancellationToken cancellationToken)
        {
            try
            {
                var response = await httpClient.GetAsync(url, cancellationToken);
                var result = HttpHelper.CheckStatusCode(response);
                if (!result.IsSuccess)
                    return result;
                var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
                var opt = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var obj = JsonSerializer.Deserialize<T>(responseString, opt);
                if (obj == null)
                    return Result.Invalid(new List<ValidationError>{
                        new ValidationError
                    {
                        Identifier = nameof(responseString),
                        ErrorMessage = $"{responseString} can't parsed to {nameof(T)}"
                    }
                    });
                return obj;
            }
            catch (Exception e)
            {
                return Result.Error(e.Message);
            }
        }
        public static Uri ConvertQueryToUri<T>(string baseAddres, T query) where T : class
        {
            var stringBuilder = new StringBuilder(baseAddres + "?");
            var properties = query.GetType().
                GetProperties().
                Where(x => x.CanRead).
                Where(x => x.GetValue(query, null) != null).
                ToDictionary(x => x.Name, x => x.GetValue(query, null));

           
            stringBuilder.AppendJoin("&", properties.Select(property =>
            {
                var str = "";
                if (property.Value is not string && property.Value is ICollection)
                {
                    var valueType = properties.GetType();
                    var valueElemType = valueType.IsGenericType
                                            ? valueType.GetGenericArguments()[0]
                                            : valueType.GetElementType();
                    var collection = property.Value as ICollection;
                    if (collection != null && collection.Count > 0)
                       str = string.Join($"&", collection.Cast<object>().Select(c => $"{property.Key}={c}"));
                }
                else
                {
                    str =$"{property.Key}={property.Value}";
                }
                return str;
            }).Where(c=> !string.IsNullOrEmpty(c)));
            return new Uri(stringBuilder.ToString());
        }
    }
}
