using Microsoft.Extensions.Configuration;
using WPImporter.Common;
using WPImporter.Database;

namespace WPImporter
{
    public class App
    {
        private readonly IImportedDbContext _context;
        private readonly Keys _keys;

        public App(IImportedDbContext context, IConfiguration configuration)
        {
            _context = context;
            _keys = configuration.GetSection("Keys").Get<Keys>();
        }

        public void StartApplication()
        {
            // 1. Wyciągamy rekordy z bazy danych
            var companies =  _context.Companies.ToList();

            // 2. ...
            var googleApi = _keys.GoogleApiKey;
        }
    }
}