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

        private readonly int _limit = 1;

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

            var iteration = 0;

            foreach (var company in companies)
            {
                Console.WriteLine($"1. Rozpoczynam procesować firmę o ID {company.Id}");

                // 2. Pobieranie danych z Google API   
                var placeId = _google.GetPlaceId(company.Name);

                var placeDetails = _google.GetPlaceDetails(placeId);

                Console.WriteLine($"2. Pobrałem dane z Google API | Place ID {placeId}");

                // 3. Wysyłamy dane do WordPress'a
                var listing = new Listing
                {
                    Title = placeDetails.result.name,
                    Content = $"Opis przygotowany pod SEO z dynamicznami wstawkami jak ta nazwa firmy np: {placeDetails.result.name}"
                };

                var newListingUrl = _wp.AddListing(listing);

                Console.WriteLine($"3. Dodałem firmę przez WP API | URL {newListingUrl}");

                // 4. Dodajemy opinie do WordPress'a
                var bot = new Bot(newListingUrl);

                foreach (var review in placeDetails.result.reviews)
                {
                    var rating = Convert.ToInt64(Math.Floor(Convert.ToDouble(review.rating)));
                    var author = review.author_name;
                    var comment = review.text;

                    bot.AddComment(rating, author, comment, company.Name);

                    Console.WriteLine($"Dodałem komentarz autora: {author} dla firmy ID: {company.Id}");
                }

                Console.WriteLine($"4. Dodałem {placeDetails.result.reviews.Count} komentarzy");

                iteration += 1;

                if (iteration == _limit)
                {
                    break;
                }
            }
        }
    }
}