using WPImporter.Database;

namespace WPImporter
{
    public class App
    {
        private readonly IImportedDbContext _context;

        public App(IImportedDbContext context)
        {
            _context = context;
        }

        public void StartApplication()
        {
            // 1. Wyciągamy rekordy z bazy danych
            var companies =  _context.Companies.ToList();

            // 2. ...
        }
    }
}