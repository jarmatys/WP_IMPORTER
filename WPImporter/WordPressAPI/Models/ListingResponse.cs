namespace WPImporter.WordPressAPI.Models
{
    public class ListingResponse
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Date_gmt { get; set; }
        public Type Guid { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Modified_gmt { get; set; }
        public string Password { get; set; }
        public string Slug { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Link { get; set; }
        public Type Title { get; set; }
        public Content Content { get; set; }
    }

    public class Content
    {
        public string Raw { get; set; }
        public string Rendered { get; set; }
        public bool Protected { get; set; }
        public int Block_version { get; set; }
    }

    public class Type
    {
        public string Raw { get; set; }
        public string Rendered { get; set; }
    }
}
