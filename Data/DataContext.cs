using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AspWebProgram.Models;
namespace ASPWebProgramming.Data
{
    public class DataContext:IdentityDbContext<AppUser,AppRole,string>
    {
        public DataContext(DbContextOptions<DataContext> options):base (options)
        {
                
        }
        public DbSet<Doktor> Doktorlar => Set<Doktor>();
        public DbSet<Hasta> Hastalar => Set<Hasta>();
    }


}