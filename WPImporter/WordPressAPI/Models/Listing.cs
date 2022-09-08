namespace WPImporter.WordPressAPI.Models
{
    public class Listing
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Status { get; } = "publish";
        public string EnableContactWidget { get; } = "TODO: rozkminić wartość";

        // TODO: GUS PROPERTIES
        public string Nip { get; set; }
        public string Krs { get; set; }
        public string LegalForm { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Commune { get; set; }
        public string Disctrict { get; set; }
        public string Street { get; set; }
        public string FlatNumber { get; set; }
        public string BuildingNumber { get; set; }
        public string StartDate { get; set; }
        // public string ClassifiationSchema { get; set; }

        // TODO: Google Properties
        public string PlaceId { get; set; }
        public string FriendlyAddress { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Email { get; set; } // Też z GUSu może być
        public string WWW { get; set; } // Też z GUSu może być
        public string PhoneNumber { get; set; } // Też z GUSu może być
        public string OpeningHours { get; set; }
        public string YoutubeUrl { get; set; }

        // TODO: Inne
        public string Category { get; set; } // Id Kategorii z WordPress'a
        public string Features { get; set; } // Id'ki ficzerów z WordPress'a
        public string District { get; set; } // Województwo po Id z WordPress'a
        public string Keywords { get; set; } // Województwo po Id z WordPress'a


        public override string ToString()
        {
            return $"{{\"title\":\"{Title}\",\"content\":\"{Content}\",\"status\":\"{Status}\"}}";
        }
    }
}
