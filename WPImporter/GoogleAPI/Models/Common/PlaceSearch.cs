namespace WPImporter.GoogleAPI.Models.Common
{
    public class PlaceSearch
    {
        public PlaceSearch(string name, string city, string street, string buildingNumber, string? flatNumber, string postalCode)
        {
            Name = FormatName(name);
            Street = FormatStreet(street);
            City = FormatCity(city);
            BuildingNumber = FormatBuildingNumber(buildingNumber);
            FlatNumber = flatNumber;
            PostalCode = postalCode;
        }
        
        public string Name { get; }
        public string City { get; }
        public string? Street { get; }
        public string BuildingNumber { get; }
        public string? FlatNumber { get; }
        public string PostalCode { get; }

        public string GetSearchQuery()
        {
            var searchQuery = $"{Name} w {City} ul. {Street} {BuildingNumber}";

            return searchQuery;
        }

        public List<string> GetSplitedName()
        {
            var splitedName = Name.Split(" ").ToList();

            return splitedName;
        }

        private static string FormatName(string name)
        {
            return name.Replace(" SPÓŁKA Z OGRANICZONĄ ODPOWIEDZIALNOŚCIĄ", "").ToLower();
        }

        private static string FormatCity(string city)
        {
            return city.Trim().ToLower();
        }

        private static string? FormatStreet(string? street)
        {
            return street?.Replace("ul. ", "").Trim().ToLower();
        }
        private static string FormatBuildingNumber(string buildingNumber)
        {
            return buildingNumber.Trim().ToLower();
        }
    }
}
