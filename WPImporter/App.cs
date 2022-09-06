using Microsoft.Extensions.Configuration;
using WPImporter.BotWP;
using WPImporter.Common;
using WPImporter.Database;
using WPImporter.GoogleAPI;
using WPImporter.WordPressAPI;
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

        public App(IImportedDbContext context, IConfiguration configuration)
        {
            _context = context;

            _keys = configuration.GetSection("Keys").Get<Keys>();
            _wpc = configuration.GetSection("WordPressConfig").Get<WordPressConfig>();

            _google = new Google(_keys.GoogleApiKey);
            _wp = new WordPress(_wpc.Url, _wpc.UserName, _wpc.Password);
        }

        public void StartApplication()
        {
            // 1. Wyciągamy rekordy z bazy danych
            var companies = _context.Companies.ToList();

            // 2. Pobieranie danych z Google API   
            var placeId = _google.GetPlaceId("armatys.me Jarosław Armatys");

            var placeDetails = _google.GetPlaceDetails(placeId);

            // 3. Wysyłamy dane do WordPress'a
            var listing = new Listing
            {
                Title = placeDetails.result.name,
                Content = $"Opis przygotowany pod SEO z dynamicznami wstawkami jak ta nazwa firmy np: {placeDetails.result.name}"
            };

            var newListingUrl = _wp.AddListing(listing);

            // 4. Dodajemy opinie do WordPress'a
            var bot = new Bot(newListingUrl);

            var rating = 4;
            var author = "Author Name";
            var comment = "Review Text";

            bot.AddComment(rating, author, comment, "armatys.me Jarosław Armatys");
        }
    }
}