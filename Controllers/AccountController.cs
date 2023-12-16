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
            var user = db.Adminler.FirstOrDefault(u => u.KullaniciAdi == model.Username && u.Sifre == model.Password);
           if (user != null)
            {
                HttpContext.Session.SetString("Username", user.KullaniciAdi);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Kullanici adi veya şifre hatali.");
        }
        return View(model);
    }
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Oturumu temizle
        return RedirectToAction("Index", "Home"); // Anasayfaya yönlendir
    }
    }
}