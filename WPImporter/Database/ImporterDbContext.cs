using Microsoft.EntityFrameworkCore;
using WPImporter.Models;

namespace WPImporter.Database
{
    public class ImporterDbContext : DbContext, IImportedDbContext
    {
        public ImporterDbContext(DbContextOptions<ImporterDbContext> options) : base(options)
        {
        }
        
        public DbSet<Company> Companies { get; set; }
    }
}
