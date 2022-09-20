namespace WPImporter.Database.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string? Krs { get; set; }
        public string Nip { get; set; }
        public string Name { get; set; }
        public string PostalCode { get; set; }
        public string District { get; set; }
        public string Commune { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string? FlatNumber { get; set; }
        public string? MainPkd { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? WebsiteUrl { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int LegalFormId { get; set; }
        public LegalForm LegalForm { get; set; }
        
        public int ClassificationSchemaId { get; set; }
        public ClassificationSchema ClassificationSchema { get; set; }
        
        public int VoivodeshipId { get; set; }
        public Voivodeship Voivodeship { get; set; }
    }
}
