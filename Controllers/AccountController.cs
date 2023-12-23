using ASPWebProgramming.Data;
using Microsoft.AspNetCore.Mvc;
using AspWebProgram.Models;
using AspWebProgram.Models;
using Microsoft.AspNetCore.Identity;

namespace Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcıyı bul
                var user = await _userManager.FindByNameAsync(model.Username);

                if (user != null)
                {
                    // Şifreyi doğrula ve oturum aç
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
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
                var user = new AppUser
                {
                    UserName = model.HastaEposta, // Identity için gerekli
                    Email = model.HastaEposta,
                    HastaTc = model.HastaTc,
                    HastaAd = model.HastaAd,
                    HastaSoyad = model.HastaSoyad,
                    HastaTel = model.HastaTel,
                    HastaCinsiyet = model.HastaCinsiyet,
                    Password = model.Password

                };

                // Kullanıcıyı oluştur
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (model.HastaEposta == "g211210028@sakarya.edu.tr")
                    {
                        // Kullanıcı şifresini değiştir
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var passwordResult = await _userManager.ResetPasswordAsync(user, token, "sau");

                        if (passwordResult.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(user, "Admin");
                        }
                        else
                        {
                            // Şifre değiştirme işlemi sırasında hata oluşursa
                            foreach (var error in passwordResult.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                            // Kullanıcı oluşturma işlemini geri alabilirsiniz
                            await _userManager.DeleteAsync(user);
                            return View(model);
                        }
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }

                    // Kullanıcı başarıyla oluşturulduktan sonra başka bir sayfaya yönlendir
                    return RedirectToAction("Index", "Home");
                }
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