using MySqlConnector;
using Newtonsoft.Json;
using RestSharp;
using WPImporter.Common;
using WPImporter.WordPressAPI.Models;

namespace WPImporter.WordPressAPI
{
    public class WordPress
    {
        private readonly string _baseUrl;
        private readonly string _bearerToken;
        private readonly string _connectionString;

        public WordPress(WordPressConfig config)
        {
            _baseUrl = config.Url;
            _bearerToken = GetBearerToken(config.UserName, config.Password);
            _connectionString = config.DataBaseConnectionString;
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

        public ListingResponse AddListing(Listing listing)
        {
            var url = $"{_baseUrl}/wp-json/wp/v2/listing";

            var client = new RestClient(url);

            var request = new RestRequest
            {
                Method = Method.Post
            };

            var payload = JsonConvert.SerializeObject(listing); // TODO: dodać to lower case pattern

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {_bearerToken}");
            request.AddParameter("application/json", payload, ParameterType.RequestBody);

            var response = client.Execute(request);

            var deserializeResponse = JsonConvert.DeserializeObject<ListingResponse>(response.Content);

            return deserializeResponse;
        }

        public void AddMetaDatas(int listingId, ListingMetaData listingMetaData)
        {
            using var connection = new MySqlConnection(_connectionString);

            connection.Open();

            ExecuteSqlQuery(connection, listingId.ToString(), "_listing_type", listingMetaData.ListingType);

            foreach (var metaData in listingMetaData.MetaDatas)
            {
                ExecuteSqlQuery(connection, listingId.ToString(), metaData.MetaIdentifier, metaData.MetaValue);
            }

            connection.Close();
        }

        private void ExecuteSqlQuery(MySqlConnection connection, string listingId, string metaIdentifier, string metaValue)
        {
            var sqlCommand = "INSERT INTO `wp_postmeta` (`post_id`, `meta_key`, `meta_value`) VALUES (@post_id, @meta_key, @meta_value)";

            using var command = new MySqlCommand(sqlCommand, connection);

            command.Parameters.AddWithValue("@post_id", listingId);
            command.Parameters.AddWithValue("@meta_key", metaIdentifier);
            command.Parameters.AddWithValue("@meta_value", metaValue);

            command.ExecuteNonQuery();
        }
    }
}
