using ASPWebProgramming.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspWebProgram.Data
{
    public static class IdentitySeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "sau";

        public static async void IdentityTestUser(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<DataContext>>();
            var user = await userManager.FindByNameAsync(adminUser);

            // if (user==null)
            // {
            //     user = new AppUser{
            //         FullName="admin",
            //         UserName = adminUser,
            //         Email="g201210011@sakarya.edu.tr",
            //         PhoneNumber="21212121"
            //     };

            //     await userManager.CreateAsync(user,adminPassword);
            // }
        }
    }
}