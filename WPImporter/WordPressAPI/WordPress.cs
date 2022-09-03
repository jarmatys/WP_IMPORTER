using Newtonsoft.Json;
using RestSharp;
using WPImporter.WordPressAPI.Models;

namespace WPImporter.WordPressAPI
{
    public class WordPress
    {
        private readonly string _baseUrl;
        private readonly string _bearerToken;

        public WordPress(string url, string userName, string password)
        {
            _baseUrl = url;
            _bearerToken = GetBearerToken(userName, password);
        }

        private string GetBearerToken(string userName, string password)
        {
            var url = $"{_baseUrl}/wp-json/jwt-auth/v1/token";
            var login = new LoginRequest { UserName = userName, Password = password };

            var client = new RestClient(url);

            var request = new RestRequest
            {
                Method = Method.Post
            };
            
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", login.ToString(), ParameterType.RequestBody);
            
            var response = client.Execute(request);

            var deserializeResponse = JsonConvert.DeserializeObject<LoginResponse>(response.Content);

            return deserializeResponse.Token;
        }

        public string AddListing(Listing listing)
        {
            var url = $"{_baseUrl}/wp-json/wp/v2/listing";
            
            var client = new RestClient(url);

            var request = new RestRequest
            {
                Method = Method.Post
            };

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {_bearerToken}");
            request.AddParameter("application/json", listing.ToString(), ParameterType.RequestBody);

            var response = client.Execute(request);

            var deserializeResponse = JsonConvert.DeserializeObject<ListingResponse>(response.Content);

            return deserializeResponse.Id;
        }
    }
}
    