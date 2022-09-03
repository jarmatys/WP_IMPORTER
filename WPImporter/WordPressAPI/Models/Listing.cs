namespace WPImporter.WordPressAPI.Models
{
    public class Listing
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Status { get; } = "publish";

        public override string ToString()
        {
            return $"{{\"title\":\"{Title}\",\"content\":\"{Content}\",\"status\":\"{Status}\"}}";
        }
    }
}
