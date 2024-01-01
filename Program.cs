using System.Globalization;
using System.Reflection;
using System.Text;
using AspWebProgram.Services;
using AspWebProgramming.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
#region Localizer
builder.Services.AddSingleton<LanguageServices>();
builder.Services.AddLocalization(options=>options.ResourcesPath="Resources");
builder.Services.AddMvc().AddViewLocalization().AddDataAnnotationsLocalization(options=>options.DataAnnotationLocalizerProvider=(type,factory)=>{
    var assemblyName=new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
    return factory.Create(nameof(SharedResource),assemblyName.Name);
});
builder.Services.Configure<RequestLocalizationOptions>(options=>
{
    var supportCultures=new List<CultureInfo>{
        new CultureInfo("en-US"),
        new CultureInfo("tr-TR")
    };
    options.DefaultRequestCulture=new RequestCulture(culture : "tr-TR",uiCulture: "tr-TR");
    options.SupportedCultures=supportCultures;
    options.SupportedUICultures=supportCultures;
    options.RequestCultureProviders.Insert(0,new QueryStringRequestCultureProvider());
});
#endregion

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
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Users/SignIn";
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});
builder.Services.ConfigureApplicationCookie(options=>{
    options.LoginPath="/Account/Login";
    //options.AccessDeniedPath="/Account/AccessDenied";
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
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
