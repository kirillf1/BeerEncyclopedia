using Ardalis.Result;
using BeerFormaters.BeerNameFormater;
using HtmlAgilityPack;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace BeerEncyclopedia.Application.ImageSearchServices
{
    public class YandexNameByImageSearchService : IImageSearchSevice
    {
        private const string BaseAddress = "https://yandex.ru/";
        private readonly HttpClient httpClient;

        public YandexNameByImageSearchService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<Result<IEnumerable<string>>> GetNamesByImage(byte[] imageBytes)
        {
            try
            {
                using var content = new ByteArrayContent(imageBytes);
                var response = await httpClient.
                  PostAsync(BaseAddress + "images-apphost/image-download?cbird=111" +
                  "&images_avatars_size=preview&images_avatars_namespace=images-cbir", content);
                if (!response.IsSuccessStatusCode)
                    return Result.Error($"Request failed with code {(int)response.StatusCode}");
                var pictureInfo = await response.Content.ReadFromJsonAsync<YandexPictureInfo>
                    (new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                if (pictureInfo == null)
                    return Result.NotFound();
                response = await httpClient.GetAsync(BaseAddress + $"images/" +
                   $"search?rpt=imageview&url={pictureInfo.Url}&cbir_id={pictureInfo.ImageShard}/{pictureInfo.ImageId}");
                if (!response.IsSuccessStatusCode)
                    return Result.Error($"Request failed with code {(int)response.StatusCode}");
                var html = await response.Content.ReadAsStringAsync();
                var document = new HtmlDocument();
                document.LoadHtml(html);
                var names = document.DocumentNode.SelectNodes("(//a[@class='Button2 Button2_size_l " +
                    "Button2_type_link Button2_view_default Button2_width_auto Tags-Item'])")?
                    .Select(c => c.InnerText)?.ToList();
                if (names == null)
                    return Result.NotFound();
                return Result.Success(names
                    .Distinct()
                    .Where(n => !string.IsNullOrEmpty(n)));
            }
            catch (Exception e)
            {
                return Result.Error(e.Message);
            }
        }
        private class YandexPictureInfo
        {

            [JsonPropertyName("image_id")]
            public string ImageId { get; set; }

            [JsonPropertyName("url")]
            public string Url { get; set; }

            [JsonPropertyName("image_shard")]
            public int ImageShard { get; set; }

            [JsonPropertyName("height")]
            public int Height { get; set; }

            [JsonPropertyName("width")]
            public int Width { get; set; }

            [JsonPropertyName("namespace")]
            public string Namespace { get; set; }
        }
    }
}
