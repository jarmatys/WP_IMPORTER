using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WPImporter.BotWP;
using WPImporter.Common;
using WPImporter.Database;
using WPImporter.GoogleAPI;
using WPImporter.GoogleAPI.Models.Common;
using WPImporter.WordPressAPI;
using WPImporter.WordPressAPI.Helpers;
using WPImporter.WordPressAPI.Models;

namespace WPImporter
{
    public class App
    {
        private readonly IImportedDbContext _context;

        private readonly Keys _keys;
        private readonly WordPressConfig _wpc;

        private readonly Google _google;
        private readonly WordPress _wp;

        private readonly int _limit = 1;
        private readonly string MAIN_PKD = "4321Z"; // GŁÓWNY KOD PKD FOTOWOLTAIKI

        public App(IImportedDbContext context, IConfiguration configuration)
        {
            _context = context;

            _keys = configuration.GetSection("Keys").Get<Keys>();
            _wpc = configuration.GetSection("WordPressConfig").Get<WordPressConfig>();

            _google = new Google(_keys.GoogleApiKey);
            _wp = new WordPress(_wpc);
        }

        public void StartApplication()
        {
            // 1. Wyciągamy rekordy z bazy danych
            var companies = _context
                .Companies
                .Include(c => c.ClassificationSchema)
                .Include(c => c.Voivodeship)
                .Include(c => c.LegalForm)
                .Where(c => c.MainPkd == MAIN_PKD && c.Name.ToLower().Contains("foto"))
                .Skip(0)
                .Take(_limit)
                .ToList();

            var iteration = 0;

            foreach (var company in companies)
            {
                Console.WriteLine($"{iteration} | {company.Name}");

                // 2. Pobieranie danych z Google API   
                var placeSearch = new PlaceSearch(company.Name, company.City, company.Street, company.BuildingNumber, company.FlatNumber, company.PostalCode);

                var placeId = _google.GetPlaceIdAdvanced(placeSearch);

                var placeDetails = placeId != null ? _google.GetPlaceDetails(placeId) : null;

                Console.WriteLine($"Pobrałem dane z Google API | Place ID: {placeId != null} | Place Details: {placeDetails != null}");

                // 3. Wysyłamy dane do WordPress'a
                var listing = new Listing
                {
                    Title = $"{company.Name}",
                    Content = WordPressHelper.CreateCompanyDescription(company),
                    ListingCategory = 33, // TODO: Dynamiczne ustawienie kategorii
                    ListingFeature = new List<int> { 62, 64 },
                    Region = WordPressHelper.GetRegionId(company.VoivodeshipId), 
                };

                var newListing = _wp.AddListing(listing);

                Console.WriteLine($"Dodałem firmę przez WP API | URL {newListing.Link}");

                var listingMetaDate = WordPressHelper.CreateListingMetaData(company, placeDetails, placeId);

                _wp.AddMetaDatas(newListing.Id, listingMetaDate);

                Console.WriteLine($"Dodałem meta dane do ogłoszenia | ID {newListing.Id}");

                // 4. Dodajemy opinie do WordPress'a
                var bot = new Bot(newListing.Link);

                if (placeDetails != null && placeDetails.result.reviews != null)
                {
                    foreach (var review in placeDetails.result.reviews)
                    {
                        var rating = Convert.ToInt64(Math.Floor(Convert.ToDouble(review.rating)));
                        var author = review.author_name; // TODO: zrobić anonimizację
                        var comment = review.text;

                        bot.AddComment(rating, author, comment, company.Name);

                        Console.WriteLine($"Dodałem komentarz autora: {author} dla firmy ID: {company.Id}");
                    }

                    Console.WriteLine($"Dodałem {placeDetails.result.reviews.Count} komentarzy");
                }
                else
                {
                    Console.WriteLine($"Brak komentarzy");
                }

                iteration += 1;
            }
        }
    }
}