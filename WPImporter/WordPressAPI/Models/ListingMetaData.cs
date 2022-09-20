namespace WPImporter.WordPressAPI.Models
{
    public class ListingMetaData
    {
        public string ListingType { get; } = "service";

        public List<MetaData> MetaDatas { get; set; }
    }
}
