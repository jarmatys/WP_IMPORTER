using Microsoft.EntityFrameworkCore;
using WPImporter.Database;

namespace WPImporter
{
    public class App
    {
        private readonly ImporterDbContext _context;

        public App(ImporterDbContext context)
        {
            _context = context;
        }

        public async Task StartImport()
        {
            // 1. Wyciągamy rekordy z bazy danych
            var companies = await _context.Companies.ToListAsync();

            // 2. ...
        }
    }
}