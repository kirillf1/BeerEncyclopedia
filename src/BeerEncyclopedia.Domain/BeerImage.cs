namespace BeerEncyclopedia.Domain
{
    public class BeerImages
    {
        private BeerImages() { }
        public BeerImages(string url)
        {
            MainImageUrl = url;
            ImageUrls = new List<string>();
        }
        public string MainImageUrl { get; set; }
        public List<string> ImageUrls { get; set; }
    }
}
