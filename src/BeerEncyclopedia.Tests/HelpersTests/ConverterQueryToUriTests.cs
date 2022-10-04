using BeerEncyclopedia.Application.Helpers;

namespace BeerEncyclopedia.Tests.HelpersTests
{
    public class ConverterQueryToUriTests
    {
        [Fact]
        public void ConvertQueryToUri_WithNotEmptyCollection_EqualsExpectedString()
        {
            var id = Guid.NewGuid();
            var query = new Query();
            query.Param.AddRange(new int[] { 1, 2 });
            query.Ids.Add(id);
            string domain = "http://localhost:5000/";
            var expectedString = $"{domain}?PageIndex=10&Ids={id}&Param={1}&Param={2}";

            var result = HttpHelper.ConvertQueryToUri(domain, query);

            Assert.Equal(expectedString, result.ToString());
        }
        [Fact]
        public void ConvertQueryToUri_WithEmptyCollection_EqualsExpectedString()
        {
            var query = new Query();
            string domain = "http://localhost:5000/";
            var expectedString = $"{domain}?PageIndex=10";

            var result = HttpHelper.ConvertQueryToUri(domain, query);

            Assert.Equal(expectedString, result.ToString());
        }
        private class Query
        {
            public int PageIndex { get; set; } = 10;
            public List<Guid> Ids { get; set; } = new List<Guid>();
            public List<int> Param { get; set; } = new List<int>();
        }
    }
}
