using AspWebProgramming.Data;
using Microsoft.AspNetCore.Mvc;
using AspWebProgram.Models;

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
                // Önce adminler arasında kontrol et
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
                    return RedirectToAction("Index", "Home");
                }
                else if (hastaUser != null)
                {
                    if (hastaUser != null)
                    {
                        HttpContext.Session.SetString("Username", hastaUser.HastaAd);
                        HttpContext.Session.SetString("UserRole", "Hasta"); // Örnek bir rol ataması
                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (doktorUser != null)
                {
                    HttpContext.Session.SetString("Username", doktorUser.DoktorTc);
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

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Oturumu temizle
            HttpContext.Session.Remove("UserRole");
            return RedirectToAction("Index", "Home"); // Anasayfaya yönlendir
        }
    }
}