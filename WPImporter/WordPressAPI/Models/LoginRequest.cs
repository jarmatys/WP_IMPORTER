namespace WPImporter.WordPressAPI.Models
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return $"{{\"username\":\"{UserName}\",\"password\":\"{Password}\"}}";
        }
    }
}
