using Microsoft.EntityFrameworkCore;
using WPImporter.Models;

namespace WPImporter.Database
{
    public interface IImportedDbContext
    {
        DbSet<Company> Companies { get; set; }
    }
}
