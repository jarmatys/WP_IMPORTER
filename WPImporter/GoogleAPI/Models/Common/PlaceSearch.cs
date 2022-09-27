namespace WPImporter.GoogleAPI.Models.Common
{
    public class PlaceSearch
    {
        public PlaceSearch(string name, string? city, string? street, string? buildingNumber, string? flatNumber, string? postalCode)
        {
            Name = FormatName(name);
            Street = FormatStreet(street);
            City = FormatCity(city);
            BuildingNumber = FormatBuildingNumber(buildingNumber);
            FlatNumber = FormatFlatNumber(flatNumber);
            PostalCode = FormatPostalCode(postalCode);
        }

        public string Name { get; }
        public string City { get; }
        public string Street { get; }
        public string BuildingNumber { get; }
        public string FlatNumber { get; }
        public string PostalCode { get; }

        public string GetAddress()
        {
            var address = $"{Street} {BuildingNumber}";

            if (FlatNumber != null)
            {
                address += $"/{FlatNumber}";
            }

            address += $", {PostalCode} {City}";

            return address;
        }
        
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

        private static string FormatCity(string? city)
        {
            if (string.IsNullOrEmpty(city))
                return "";
            
            return city.Trim().ToLower();
        }

        private static string FormatFlatNumber(string? flatNumber)
        {
            if (string.IsNullOrEmpty(flatNumber))
                return "";

            return flatNumber.Trim().ToLower();
        }

        private static string FormatPostalCode(string? postalCode)
        {
            if (string.IsNullOrEmpty(postalCode))
                return "";

            return postalCode.Trim().ToLower();
        }

        private static string FormatStreet(string? street)
        {
            if (string.IsNullOrEmpty(street))
                return "";
            
            return street.Replace("ul. ", "").Trim().ToLower();
        }
        private static string FormatBuildingNumber(string? buildingNumber)
        {
            if (string.IsNullOrEmpty(buildingNumber))
                return "";
            
            return buildingNumber.Trim().ToLower();
        }
    }
}
