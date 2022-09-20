using Newtonsoft.Json;

namespace WPImporter.WordPressAPI.Models
{
    public class Listing
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("status")]
        public string Status { get; } = "publish";

        [JsonProperty("listing_category")]
        public int ListingCategory { get; set; }

        [JsonProperty("listing_feature")]
        public List<int> ListingFeature { get; set; }

        [JsonProperty("region")]
        public int Region { get; set; }
    }
}
