using ASPWebProgramming.Data;
using Microsoft.AspNetCore.Mvc;
using AspWebProgram.Models;
using Microsoft.AspNetCore.Identity;
using System.Data.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Controllers
{

    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;
        private RoleManager<AppRole> _roleManager;
        private SignInManager<AppUser> _signInManager;
        private DataContext db;
        public AccountController(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            SignInManager<AppUser> signInManager,DataContext _context
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            db=_context;
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
                var user = await _userManager.FindByEmailAsync(model.Username);

                if (user != null)
                {
                    await _signInManager.SignOutAsync();
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
                //await EnsureRolesCreated();
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
                var hasta = new Hasta
                {
                    HastaTc = model.HastaTc,
                    HastaAd = model.HastaAd,
                    HastaSoyad = model.HastaSoyad,
                    HastaTel = model.HastaTel,
                    HastaEposta = model.HastaEposta,
                    HastaCinsiyet = model.HastaCinsiyet,
                    HastaSifre = model.Password
                };
                // Kullanıcıyı oluştur
                var result = await _userManager.CreateAsync(user, model.Password);
                db.Hastalar.Add(hasta);
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

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home"); // Anasayfaya yönlendir
        }
    }
}