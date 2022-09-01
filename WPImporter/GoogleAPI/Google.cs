using Newtonsoft.Json;
using WPImporter.GoogleAPI.Models;

namespace WPImporter.GoogleAPI
{
    public class Google
    {
        private readonly string API_KEY;
        private readonly string BASE_URL = "https://maps.googleapis.com/maps/api/place/";

        public Google(string apiKey)
        {
            API_KEY = apiKey;
        }
        
        public string GetPlaceId(string searchQuery)
        {
            string url = BASE_URL + "textsearch/json?query=" + searchQuery + "&key=" + API_KEY;
            
            var response = GetResponse(url);
            
            var placeBasic = JsonConvert.DeserializeObject<Place>(response);

            return placeBasic.results[0].place_id;
        }

        public PlaceDetails GetPlaceDetails(string placeId)
        {
            string url = BASE_URL + "details/json?place_id=" + placeId + "&language=pl&key=" + API_KEY;

            var response = GetResponse(url);

            var placeDetails = JsonConvert.DeserializeObject<PlaceDetails>(response);
            
            return placeDetails;
        }

        private static string GetResponse(string url)
        {
            using var client = new HttpClient();
            
            var response = client.GetAsync(url).Result;
            
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
