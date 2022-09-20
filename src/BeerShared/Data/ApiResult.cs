using System.Text.Json.Serialization;

namespace BeerShared.Data
{
    public class ApiResult<T>
    {
        [JsonConstructor]
        public ApiResult(
            List<T> data,
            int totalCount,
            int pageIndex,
            int pageSize)
        {
            Data = data;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
        [JsonInclude]
        public List<T> Data { get; init; }
        [JsonInclude]
        public int PageIndex { get; init; }
        [JsonInclude]
        public int PageSize { get; init; }
        [JsonInclude]
        public int TotalCount { get; init; }
        [JsonInclude]
        public int TotalPages { get; init; }
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }
        public bool HasNextPage
        {
            get
            {
                return ((PageIndex + 1) < TotalPages);
            }
        }
    }
}
