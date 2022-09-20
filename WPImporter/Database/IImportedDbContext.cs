using Microsoft.EntityFrameworkCore;
using WPImporter.Database.Models;

namespace WPImporter.Database
{
    public interface IImportedDbContext
    {
        DbSet<Company> Companies { get; set; }
        DbSet<LegalForm> LegalForms { get; set; }
        DbSet<Voivodeship> Voivodeships { get; set; }
        DbSet<ClassificationSchema> ClassificationSchemas { get; set; }
    }
}
