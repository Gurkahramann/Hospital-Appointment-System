using System.Net;
using AspWebProgramming.Data;
using Microsoft.AspNetCore.Mvc;
using AspWebProgram.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Controllers
{

    public class AccountController : Controller
    {
        private DataContext db;

        public AccountController(DataContext context)
        {
            db = context;
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUsername = HttpContext.Session.GetString("Username");
                var currentUserRole = HttpContext.Session.GetString("UserRole");

                if (!string.IsNullOrEmpty(currentUsername) && !string.IsNullOrEmpty(currentUserRole))
                {
                    // Kullanıcı zaten oturum açmış
                    return Unauthorized();
                }
                var adminUser = db.Adminler
                    .FirstOrDefault(u => u.KullaniciAdi == model.Username && u.Sifre == model.Password);
                var hastaUser = db.Hastalar
                        .FirstOrDefault(h => h.HastaTc == model.Username &&
                         h.HastaSifre == model.Password);
                var doktorUser = db.Doktorlar.FirstOrDefault(u => u.DoktorTc == model.Username && u.DoktorSifre == model.Password);
                if (adminUser != null)
                {
                    HttpContext.Session.SetString("Username", adminUser.KullaniciAdi);
                    HttpContext.Session.SetString("UserRole", "Admin"); // Örnek bir rol ataması
                    var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Role,"Admin")
                };
                    return RedirectToAction("Index", "Home");
                }
                else if (hastaUser != null)
                {
                    if (hastaUser != null)
                    {
                        HttpContext.Session.SetString("Username", hastaUser.HastaAd);
                        HttpContext.Session.SetString("LoginName", hastaUser.HastaTc);
                        HttpContext.Session.SetString("UserRole", "Hasta"); // Örnek bir rol ataması
                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (doktorUser != null)
                {
                    HttpContext.Session.SetString("Username", doktorUser.DoktorAd);
                    HttpContext.Session.SetString("LoginName", doktorUser.DoktorTc);
                    HttpContext.Session.SetString("UserRole", "Doktor"); // Örnek bir rol ataması
                    return RedirectToAction("Index", "Home");
                }
                // Kullanıcı adı veya şifre hatalıysa
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
            }


            return View(model);
        }




        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.HastaEposta == "g211210028@sakarya.edu.tr")
                {
                    // Admin olarak kaydet
                    // var existingUser = await db.Adminler.FirstOrDefaultAsync(h => h.KullaniciAdi == model.HastaEposta);
                    // if (existingUser != null)
                    // {
                    //     // Aynı e-posta ile kayıtlı bir kullanıcı varsa, uygun bir hata mesajıyla geri dön
                    //     ModelState.AddModelError(string.Empty, "Bu e-posta adresi zaten kullanılmaktadır.");
                    //     return View(model);
                    // }
                    var admin = new Admin
                    {
                        KullaniciAdi = model.HastaEposta, // Kullanıcı adı olarak adı kullanabilirsiniz
                        Sifre = "sau", // Şifre varsayılan olarak "sau"
                        Rol = "Admin" // Admin rolünü ata

                        // Diğer gereken alanlar...
                    };
                    // var claims = new List<Claim>
                    // {
                    // new Claim(ClaimTypes.Role, "Admin"), // veya "Doktor", "Admin" gibi kullanıcının rolü.
                    //  };
                    // var claimsIdentity = new ClaimsIdentity(
                    // claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    // var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    // // Kullanıcıyı oturum açtır.
                    // await HttpContext.SignInAsync(
                    //     CookieAuthenticationDefaults.AuthenticationScheme,
                    //     claimsPrincipal);
                    db.Adminler.Add(admin);
                }
                else
                {
                    // var existingUser = await db.Hastalar.FirstOrDefaultAsync(h => h.HastaTc == model.HastaTc);
                    // if (existingUser != null)
                    // {
                    //     // Aynı e-posta ile kayıtlı bir kullanıcı varsa, uygun bir hata mesajıyla geri dön
                    //     ModelState.AddModelError(string.Empty, "Bu TCN kullanılmaktadır.");
                    //     return View(model);
                    // }
                    var hasta = new Hasta
                    {
                        HastaTc = model.HastaTc,
                        HastaAd = model.HastaAd,
                        HastaSoyad = model.HastaSoyad,
                        HastaTel = model.HastaTel,
                        HastaEposta = model.HastaEposta,
                        HastaCinsiyet = model.HastaCinsiyet,
                        HastaSifre = model.Password,
                        Rol = "Hasta"
                    };
                    var claims = new List<Claim>
                    {
                    new Claim(ClaimTypes.Role, "Hasta"), // veya "Doktor", "Admin" gibi kullanıcının rolü.
                    };
                    var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    // Kullanıcıyı oturum açtır.
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        claimsPrincipal);

                    // Hasta entity'sini veritabanına ekle
                    db.Hastalar.Add(hasta);
                }
                // ViewModel verilerini Hasta entity'sine dönüştür

                await db.SaveChangesAsync(); // Değişiklikleri kaydet

                // Başarılı kayıttan sonra kullanıcıyı başka bir sayfaya yönlendir
                return RedirectToAction("Index", "Home");
            }

            // Eğer model geçersizse, formu tekrar göster
            return View(model);
        }
        public ActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            // Oturum bilgilerini temizle
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Herhangi bir ekstra oturum bilgisi varsa, bu da temizlenebilir.
            HttpContext.Session.Clear();

            // Anasayfaya yönlendir
            return RedirectToAction("Index", "Home");
        }
    }
}