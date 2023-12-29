using AspWebProgramming.Data;
using Microsoft.AspNetCore.Mvc;
using AspWebProgram.Models;
using Microsoft.EntityFrameworkCore;

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
            return View(await db.Doktorlar.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDoktorModel model)
        {
            if (ModelState.IsValid)
            {
                // Veritabanına doktor kaydını eklemek için modeli kullanabilirsiniz.
                var doktor = new Doktor
                {
                    DoktorTc = model.DoktorTc,
                    DoktorAd = model.DoktorAd,
                    DoktorSoyad = model.DoktorSoyad,
                    DoktorCinsiyet = model.DoktorCinsiyet,
                    DoktorSifre = model.Password,
                    DoktorBrans = model.DoktorBrans,
                    DoktorAnaBilim = model.SelectedAnaBilimId.ToString(), // Seçilen Ana Bilim Id'sini string olarak ayarla
                    DoktorPoliklinik = model.SelectedPoliklinikId.ToString(), // Seçilen Poliklinik Id'sini string olarak ayarla
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
                // Değişiklikleri kaydet
                db.AnaBilimler.Add(anaBilim);
                db.Poliklinikler.Add(poliklinik);
                db.Doktorlar.Add(doktor);
                await db.SaveChangesAsync();

                // Başarılı kayıttan sonra kullanıcıyı başka bir sayfaya yönlendir
                return RedirectToAction("Index", "Home");
            }

            // ModelState geçerli değilse, hata mesajlarıyla birlikte sayfayı tekrar gösterin
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
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
            if (id != doktor.DoktorId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                doktor.Rol="Doktor";
                db.Update(doktor);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(doktor);
        }
        public IActionResult Delete(int id)
        {
            var doktor = db.Doktorlar.Find(id);
            if (doktor != null)
            {
                db.Doktorlar.Remove(doktor);
                db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        // public async Task<IActionResult> Index(RegisterDoktorModel model)
        // {
        //     // var selectedAnaBilim = db.AnaBilimler.FirstOrDefault(ab => ab.AnaBilimId == model.SelectedAnaBilimId);
        //     // var selectedPoliklinik = db.Poliklinikler.FirstOrDefault(p => p.PoliklinikId == model.SelectedPoliklinikId);
        //     // var anabilim = new AnaBilim
        //     // {
        //     //     //
        //     //     AnaBilimId = model.SelectedAnaBilimId,
        //     //     AnaBilimAd = selectedAnaBilim.AnaBilimAd//Karşısına comboboxtan seçtiğim isim gelecek.
        //     // };
        //     // var poliklinikler = new Poliklinik
        //     // {
        //     //     PoliklinikId = model.SelectedPoliklinikId,
        //     //     PoliklinikAd =selectedPoliklinik.PoliklinikAd//Karşısına comboboxtan seçtiğim isim gelecek,
        //     // };
        //     if (ModelState.IsValid)
        //     {


        //             var doktor = new Doktor
        //             {
        //                 DoktorTc = model.DoktorTc,
        //                 DoktorAd = model.DoktorAd,
        //                 DoktorSoyad = model.DoktorSoyad,
        //                 DoktorCinsiyet = model.DoktorCinsiyet,
        //                 DoktorSifre = model.Password,
        //                 DoktorBrans=model.DoktorBrans,
        //                 Rol="Doktor"
        //             };

        //             db.Doktorlar.Add(doktor);
        //             // db.AnaBilimler.Add(anabilim);
        //             // db.Poliklinikler.Add(poliklinikler);
        //         // ViewModel verilerini Hasta entity'sine dönüştür

        //         await db.SaveChangesAsync(); // Değişiklikleri kaydet

        //         // Başarılı kayıttan sonra kullanıcıyı başka bir sayfaya yönlendir
        //         return RedirectToAction("Index", "Home");
        //     }

        //     return View(model);
        // }
        public ActionResult Register()
        {

            return View();
        }

    }
}