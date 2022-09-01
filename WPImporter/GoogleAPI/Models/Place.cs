namespace WPImporter.GoogleAPI.Models
{
    public class Place
    {
        public List<object> html_attributions { get; set; }
        public List<Result> results { get; set; }
        public string status { get; set; }
    }
}
