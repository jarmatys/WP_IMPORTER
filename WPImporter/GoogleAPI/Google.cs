using Newtonsoft.Json;
using System.Linq;
using WPImporter.GoogleAPI.Models;
using WPImporter.GoogleAPI.Models.Common;

namespace WPImporter.GoogleAPI
{
    public class Google
    {
        private readonly string API_KEY;
        private readonly string BASE_URL_PLACE = "https://maps.googleapis.com/maps/api/place/";
        private readonly string BASE_URL_GEOCODE = "https://maps.googleapis.com/maps/api/geocode/";

        public Google(string apiKey)
        {
            API_KEY = apiKey;
        }

        public string? GetPlaceIdBasic(PlaceSearch placeSearch)
        {
            string url = BASE_URL_PLACE + "textsearch/json?query=" + placeSearch.GetSearchQuery() + "&language=pl&type=establishment&key=" + API_KEY;

            var response = GetResponse(url);

            var placeBasic = JsonConvert.DeserializeObject<Place>(response);

            if (placeBasic?.status == "OK")
            {
                return placeBasic.results[0].place_id;
            }

            return null;
        }

        public string? GetPlaceIdAdvanced(PlaceSearch placeSearch)
        {
            string url = BASE_URL_PLACE + "textsearch/json?query=" + placeSearch.GetSearchQuery() + "&language=pl&type=establishment&key=" + API_KEY;

            var response = GetResponse(url);

            var placeBasic = JsonConvert.DeserializeObject<Place>(response);

            if (placeBasic?.status == "OK")
            {
                var matchedPlace = GetMostMatchedPlace(placeBasic.results, placeSearch);

                // TODO: Jeżeli mamy 100% ale w nazwie nic się nie zgadza, to dostrzał do REJESTR.io i sprawdzamy czy spółka nie jest na adresie wirtualnym
                // w przypadku JDG sprawdzamy unikalność adresu we własnej bazie danych

                if (matchedPlace != null)
                {
                    return matchedPlace.place_id;
                }
            }

            return null;
        }

        public Geometry GetPlaceCordinates(PlaceSearch placeSearch)
        {
            string url = $"{BASE_URL_GEOCODE}json?address={placeSearch.GetAddress()}&key={API_KEY}";

            var response = GetResponse(url);

            var placeBasic = JsonConvert.DeserializeObject<Place>(response);
            
            if (placeBasic?.status == "OK")
            {
                return placeBasic.results.First().geometry;
            }

            return null;
        }

        public PlaceDetails GetPlaceDetails(string placeId)
        {
            string url = BASE_URL_PLACE + "details/json?place_id=" + placeId + "&language=pl&key=" + API_KEY;

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

        private static Result GetMostMatchedPlace(List<Result> results, PlaceSearch placeSearch)
        {
            var posibilites = new Dictionary<Result, int>();

            foreach (var result in results)
            {
                var probability = CalculateProbability(result, placeSearch);

                posibilites.Add(result, probability);
            }

            var resultPlace = posibilites.Where(x => x.Value == 100).OrderByDescending(x => x.Value).FirstOrDefault().Key;

            // TODO: zapisywać wynik do bazy danych

            return resultPlace;
        }

        private static int CalculateProbability(Result result, PlaceSearch placeSearch)
        {
            var isEstablishment = result.types.Contains("establishment");
            if (!isEstablishment) return 0;

            var isCorrectName = result.name.ToLower().Contains(placeSearch.Name);
            if (isCorrectName) return 100;

            var propabilty = 0;

            var isCorrectCity = result.formatted_address.ToLower().Contains(placeSearch.City);
            if (isCorrectCity) propabilty += 20;

            var isCorrectStreet = !string.IsNullOrEmpty(placeSearch.Street) && result.formatted_address.ToLower().Contains(placeSearch.Street);
            if (isCorrectStreet) propabilty += 30;
            if (string.IsNullOrEmpty(placeSearch.Street)) propabilty += 5;

            var isCorrectNumber = !string.IsNullOrEmpty(placeSearch.BuildingNumber) && result.formatted_address.ToLower().Contains(placeSearch.BuildingNumber);
            if (isCorrectNumber) propabilty += 10;
            if (string.IsNullOrEmpty(placeSearch.BuildingNumber)) propabilty += 5;

            var isCorrectPostalCode = !string.IsNullOrEmpty(placeSearch.PostalCode) && result.formatted_address.ToLower().Contains(placeSearch.PostalCode);
            if (isCorrectPostalCode) propabilty += 40;
            if (string.IsNullOrEmpty(placeSearch.PostalCode)) propabilty += 5;

            return propabilty;
        }
    }
}
