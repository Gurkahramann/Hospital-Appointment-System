using System.Net;
using AspWebProgramming.Data;
using Microsoft.AspNetCore.Mvc;
using AspWebProgram.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

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
                    HttpContext.Session.SetString("UserRole", "Admin");
                    return RedirectToAction("Index", "Home");
                }
                else if (hastaUser != null)
                {
                    if (hastaUser != null)
                    {
                        HttpContext.Session.SetString("Username", hastaUser.HastaAd);
                        HttpContext.Session.SetString("LoginName", hastaUser.HastaTc);
                        HttpContext.Session.SetString("UserRole", "Hasta");
                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (doktorUser != null)
                {
                    HttpContext.Session.SetString("Username", doktorUser.DoktorAd);
                    HttpContext.Session.SetString("LoginName", doktorUser.DoktorTc);
                    HttpContext.Session.SetString("UserRole", "Doktor");
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
            }


            return View(model);
        }




        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.HastaEposta == "g211210028@sakarya.edu.tr" || model.HastaEposta == "g211210085@sakarya.edu.tr")
                {
                    var existingUser = await db.Adminler.FirstOrDefaultAsync(h => h.KullaniciAdi == model.HastaEposta);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError(string.Empty, "Bu e-posta adresi zaten kullanılmaktadır.");
                        return View(model);
                    }
                    var admin = new Admin
                    {
                        KullaniciAdi = model.HastaEposta,
                        Sifre = "sau",
                        Rol = "Admin"

                    };
                    db.Adminler.Add(admin);
                }
                else
                {
                    var existingUser = await db.Hastalar.FirstOrDefaultAsync(h => h.HastaTc == model.HastaTc);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError(string.Empty, "Bu TCN zaten kullanılmaktadır.");
                        return View(model);
                    }
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

                    db.Hastalar.Add(hasta);
                }


                await db.SaveChangesAsync();


                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
        public ActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear();


            return RedirectToAction("Index", "Home");
        }
    }
}