namespace WPImporter.WordPressAPI.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string User_email { get; set; }
        public string User_nicename { get; set; }
        public string User_display_name { get; set; }
    }
}
