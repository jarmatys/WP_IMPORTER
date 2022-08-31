using Microsoft.EntityFrameworkCore;
using WPImporter.Database.Models;

namespace WPImporter.Database
{
    public interface IImportedDbContext
    {
        DbSet<Company> Companies { get; set; }
    }
}
