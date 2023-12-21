using ASPWebProgramming.Data;
using Microsoft.AspNetCore.Mvc;
using AspWebProgram.Models;

namespace Controllers
{
    
    public class AccountController:Controller
    {
        private DataContext db;

        public AccountController(DataContext context)
        {
            db=context;
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
        
        if (adminUser != null)
        {
            HttpContext.Session.SetString("Username", adminUser.KullaniciAdi);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            // Admin bulunamazsa hastalar arasında kontrol et
            var hastaUser = db.Hastalar
                .FirstOrDefault(h => h.HastaTc == model.Username && 
                                     h.HastaSifre == model.Password);

            if (hastaUser != null)
            {
                HttpContext.Session.SetString("Username", hastaUser.HastaAd);
                return RedirectToAction("Index", "Home");
            }
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
            var admin = new Admin
            {
                KullaniciAdi = model.HastaEposta, // Kullanıcı adı olarak adı kullanabilirsiniz
                Sifre = "sau", // Şifre varsayılan olarak "sau"
                // Diğer gereken alanlar...
            };
            db.Adminler.Add(admin);
        }
        else
        {
            var hasta = new Hasta
            {
                HastaTc = model.HastaTc,
                HastaAd = model.HastaAd,
                HastaSoyad = model.HastaSoyad,
                HastaTel = model.HastaTel,
                HastaEposta = model.HastaEposta,
                HastaCinsiyet = model.HastaCinsiyet,
                HastaSifre = $"{model.HastaTc.Substring(0, 3)}{model.HastaTel.Substring(model.HastaTel.Length - 3)}"
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
        return RedirectToAction("Index", "Home"); // Anasayfaya yönlendir
    }
    }
}