using AspWebProgramming.Data;
using Microsoft.AspNetCore.Mvc;
using AspWebProgram.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
    public class DoktorController : Controller
    {
        private DataContext db;
        public DoktorController(DataContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Hasta" && userRole != "Doktor" && userRole != "Admin")
            {
                return Unauthorized();
            }
            return View(await db.Doktorlar.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDoktorModel model)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                var existingUser = await db.Doktorlar.FirstOrDefaultAsync(h => h.DoktorTc == model.DoktorTc);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Bu TCN zaten kullanılmaktadır.");
                    return View(model);
                }
                var doktor = new Doktor
                {
                    DoktorTc = model.DoktorTc,
                    DoktorAd = model.DoktorAd,
                    DoktorSoyad = model.DoktorSoyad,
                    DoktorCinsiyet = model.DoktorCinsiyet,
                    DoktorSifre = model.Password,
                    DoktorBrans = model.DoktorBrans,
                    DoktorAnaBilim = model.SelectedAnaBilimId.ToString(),
                    DoktorPoliklinik = model.SelectedPoliklinikId.ToString(),
                    Rol = "Doktor"
                };
                var anaBilim = new AnaBilim()
                {
                    AnaBilimAd = model.SelectedAnaBilimId
                };
                var poliklinik = new Poliklinik()
                {
                    PoliklinikAd = model.SelectedPoliklinikId
                };
                db.AnaBilimler.Add(anaBilim);
                db.Poliklinikler.Add(poliklinik);
                db.Doktorlar.Add(doktor);
                await db.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin" && userRole != "Doktor")
            {
                return Unauthorized();
            }
            var doktor = db.Doktorlar.Find(id);
            if (doktor == null)
            {
                return NotFound();
            }
            return View(doktor);
        }
        [HttpPost]
        public IActionResult Edit(int id, Doktor doktor)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin" && userRole != "Doktor")
            {
                return Unauthorized();
            }
            if (id != doktor.DoktorId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                doktor.Rol = "Doktor";
                db.Update(doktor);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(doktor);
        }
        public IActionResult Delete(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Unauthorized();
            }
            var doktor = db.Doktorlar.Find(id);
            if (doktor != null)
            {
                db.Doktorlar.Remove(doktor);
                db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public ActionResult Register()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Unauthorized();
            }
            return View();
        }

    }
}