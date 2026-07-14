using LicenseManagerMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LicenseManagerMinimalAPI.Data
{
    public class AppDatabase: DbContext
    {
        public AppDatabase(DbContextOptions<AppDatabase> options) : base(options)
        {
        }
        public DbSet<AppLicence> Licences { get; set; }
    }
}
