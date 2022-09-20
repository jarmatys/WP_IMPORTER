using Microsoft.EntityFrameworkCore;
using WPImporter.Database.Models;

namespace WPImporter.Database
{
    public class ImporterDbContext : DbContext, IImportedDbContext
    {
        public ImporterDbContext(DbContextOptions<ImporterDbContext> options) : base(options)
        {
        }
        
        public DbSet<Company> Companies { get; set; }
        public DbSet<LegalForm> LegalForms { get; set; }
        public DbSet<Voivodeship> Voivodeships { get; set; }
        public DbSet<ClassificationSchema> ClassificationSchemas { get; set; }
    }
}
