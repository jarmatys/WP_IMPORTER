namespace WPImporter.WordPressAPI.Models
{
    public class MetaData
    {
        public MetaData(string? metaValue, string metaIdentifier)
        {
            MetaValue = metaValue;
            MetaIdentifier = metaIdentifier;
        }

        public string? MetaValue { get; set; }
        public string MetaIdentifier { get; set; }
    }
}
