using AspWebProgramming.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum zaman aşımı süresi
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<DataContext>(options =>{
    var config= builder.Configuration;
    var connectionString=config.GetConnectionString("database");
    options.UseSqlServer(connectionString);
});
var app = builder.Build();
// async Task SeedAnaBilimAndPoliklinikData(IServiceProvider serviceProvider)
// {
//     using (var scope = serviceProvider.CreateScope())
//     {
//         var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>(); // DbContext'i uygun şekilde ayarlayın

//         // Önce AnaBilim verilerini ekleyin
//         if (!dbContext.AnaBilimler.Any())
//         {
//             var anaBilimler = new List<AnaBilim>
//             {
//                 new AnaBilim { AnaBilimAd = "Temel Tıp" },
//                 new AnaBilim { AnaBilimAd = "Dahili Tıp" },
//                 new AnaBilim { AnaBilimAd = "Cerrahi Tıp" }
//                 // İhtiyaca göre daha fazla ana bilim ekleyebilirsiniz
//             };

//             await dbContext.AnaBilimler.AddRangeAsync(anaBilimler);
//             await dbContext.SaveChangesAsync();
//         }

//         // Şimdi de Poliklinik verilerini ekleyin
//         if (!dbContext.Poliklinikler.Any())
//         {
//             var poliklinikler = new List<Poliklinik>
//             {
//                 new Poliklinik { PoliklinikAd = "Anatomi", AnaBilimId = 1 }, // Örnek olarak, AnaBilimId'yi ilgili ana bilim ile ilişkilendirin
//                 new Poliklinik { PoliklinikAd = "Fizyoloji", AnaBilimId = 1 },
//                 new Poliklinik { PoliklinikAd = "Acil Tıp", AnaBilimId = 2 },
//                 new Poliklinik { PoliklinikAd = "Kardiyoloji", AnaBilimId = 2 },
//                 new Poliklinik { PoliklinikAd = "Beyin ve Sinir Cerrahisi", AnaBilimId = 3 }
//                 // İhtiyaca göre daha fazla poliklinik ekleyebilirsiniz
//             };

//             await dbContext.Poliklinikler.AddRangeAsync(poliklinikler);
//             await dbContext.SaveChangesAsync();
//         }
//     }
// }
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
