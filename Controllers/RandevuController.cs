using System.Drawing;
using AspWebProgram.Models;
//using AspWebProgramming.Data;
using AspWebProgramming.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
    public class RandevuController : Controller
    {
        private DataContext dbcontext;
        public RandevuController(DataContext context)
        {
            dbcontext = context;
        }
        public async Task<IActionResult> IndexRandevu()
        {
            var randevular = await dbcontext.Randevular
            .Include(x => x.Doktor)
            .Include(x => x.Hasta)
            .ToListAsync();
            return View(randevular);
        }
        [HttpGet]
        public async Task<IActionResult> RandevuOlustur()
        {
            ViewBag.Hastalar = new SelectList(await dbcontext.Hastalar.ToListAsync(), "HastaId", "HastaAd");
            ViewBag.Doktorlar = new SelectList(await dbcontext.Doktorlar.ToListAsync(), "DoktorId", "DoktorAdSoyad");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RandevuOlustur(Randevu model)
        {
            dbcontext.Randevular.Add(model);
            await dbcontext.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Include ile ilişkili Hasta ve Doktor nesnelerini yükleyerek Randevu nesnesini çekin
            var randevu = dbcontext.Randevular
                .Include(r => r.Hasta) // Hasta bilgilerini dahil et
                .Include(r => r.Doktor) // Doktor bilgilerini dahil et
                .SingleOrDefault(r => r.RandevuId == id); // SingleOrDefault kullanarak belirli bir id'ye sahip randevuyu bulun

            if (randevu == null)
            {
                return NotFound();
            }

            // Hastalar ve Doktorlar için SelectList'leri hazırlayın
            ViewBag.Hastalar = new SelectList(dbcontext.Hastalar, "HastaId", "HastaAd", randevu.HastaId); // 'Ad' alanınızın adını modelinizdeki uygun alana göre değiştirin
            ViewBag.Doktorlar = new SelectList(dbcontext.Doktorlar, "DoktorId", "DoktorAd", randevu.DoktorId); // 'Ad' alanınızın adını modelinizdeki uygun alana göre değiştirin

            return View(randevu);
        }
        [HttpPost]
        public IActionResult Edit(int id, Randevu randevu)
        {
            if (id != randevu.RandevuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Track değişiklikleri
                    var randevuToUpdate = dbcontext.Randevular.Find(id);
                    if (randevuToUpdate == null)
                    {
                        return NotFound();
                    }

                    // Yalnızca belirli alanların güncellenmesini sağlamak için:
                    randevuToUpdate.HastaId = randevu.HastaId;
                    randevuToUpdate.DoktorId = randevu.DoktorId;
                    randevuToUpdate.RandevuTarih = randevu.RandevuTarih;

                    // Değişiklikleri kaydet
                    dbcontext.SaveChanges();
                    return RedirectToAction("Home", "Index"); // Veya başka bir uygun action'a yönlendir
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!dbcontext.Randevular.Any(r => r.RandevuId == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException)
                {
                    // Veritabanı güncelleme sırasında bir hata meydana geldi
                    // Hata yönetimi burada yapılabilir
                    ModelState.AddModelError("", "Bir hata oluştu ve kayıt güncellenemedi.");
                }
            }
            else
            {
                var errors = ModelState.Select(x => new { x.Key, x.Value.Errors })
                          .Where(y => y.Errors.Count > 0)
                          .ToList();

                // Hataları inceleyebilirsiniz veya loglayabilirsiniz
                foreach (var error in errors)
                {
                    foreach (var errorMessage in error.Errors)
                    {
                        // Loglama için bir hata mesajı
                        Console.WriteLine($"Key: {error.Key}, Error: {errorMessage.ErrorMessage}");
                    }
                }
                // Eğer ModelState geçerli değilse veya bir hata oluştuysa, formu tekrar doldur
                ViewBag.Hastalar = new SelectList(dbcontext.Hastalar, "HastaId", "HastaAd", randevu.HastaId); // 'Ad' alanınızın adını modelinizdeki uygun alana göre değiştirin
                ViewBag.Doktorlar = new SelectList(dbcontext.Doktorlar, "DoktorId", "DoktorAd", randevu.DoktorId); // 'Ad' alanınızın adını modelinizdeki uygun alana göre değiştirin

                return View(randevu);
            }
            return RedirectToAction("Index", "Home");
        }


    }



}