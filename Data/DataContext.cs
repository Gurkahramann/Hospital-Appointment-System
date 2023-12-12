using Microsoft.EntityFrameworkCore;
namespace ASPWebProgramming.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base (options)
        {
                
        }
        public DbSet<Admin> Adminler => Set<Admin>();
        public DbSet<Doktor> Doktorlar => Set<Doktor>();
        public DbSet<Hasta> Hastalar => Set<Hasta>();
    }


}