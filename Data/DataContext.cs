using AspWebProgramming.Data;
using Microsoft.EntityFrameworkCore;
namespace AspWebProgramming.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base (options)
        {
                
        }
        public DbSet<Admin> Adminler => Set<Admin>();
        public DbSet<Doktor> Doktorlar => Set<Doktor>();
        public DbSet<Hasta> Hastalar => Set<Hasta>();
        public DbSet<Poliklinik> Poliklinikler => Set<Poliklinik>();
        public DbSet<AnaBilim> AnaBilimler => Set<AnaBilim>();
         public DbSet<Randevu> Randevular => Set<Randevu>();

    }


}