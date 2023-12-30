using System.Drawing;
using System.Globalization;
using AspWebProgram.Models;
using AspWebProgramming.Data;
using Microsoft.AspNetCore.Authorization;
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
            var userRole = HttpContext.Session.GetString("UserRole");
            var loginName = HttpContext.Session.GetString("LoginName");

            IQueryable<Randevu> query = dbcontext.Randevular
                .Include(x => x.Doktor)
                .Include(x => x.Hasta);

            if (userRole == "Hasta")
            {
                // Eğer kullanıcı "Hasta" ise, yalnızca o kullanıcının randevularını getir
                query = query.Where(x => x.Hasta.HastaTc == loginName);
            }
            else if (userRole == "Doktor")
            {
                query = query.Where(x => x.Doktor.DoktorTc == loginName);
            }

            var randevular = await query.ToListAsync();
            return View(randevular);
        }
        // public async Task<IActionResult> IndexRandevu()
        // {
        //     var userRole = HttpContext.Session.GetString("UserRole");
        //     var loginName = HttpContext.Session.GetString("LoginName");
        //     var randevular = await dbcontext.Randevular
        //     .Include(x => x.Doktor)
        //     .Include(x => x.Hasta)
        //     .ToListAsync();
        //     return View(randevular);
        // }
        [HttpGet]
        public async Task<IActionResult> RandevuOlustur()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var loginName = HttpContext.Session.GetString("LoginName");

            if (userRole == "Hasta")
            {
                var hasta = await dbcontext.Hastalar.FirstOrDefaultAsync(h => h.HastaTc == loginName);
                if (hasta != null)
                {
                    ViewBag.Hastalar = new SelectList(new List<Hasta> { hasta }, "HastaId", "HastaAd");

                }
            }
            else
            {
                ViewBag.Hastalar = new SelectList(await dbcontext.Hastalar.ToListAsync(), "HastaId", "HastaAd");
            }


            ViewBag.Doktorlar = new SelectList(await dbcontext.Doktorlar.ToListAsync(), "DoktorId", "DoktorAdSoyad");
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetRandevuSaatleriJsonForEdit(DateTime secilenTarih, int randevuId)
        {
            var saatler = await GetRandevuSaatleriEdit(secilenTarih, randevuId);
            return Json(new SelectList(saatler, "Value", "Text"));
        }
        [HttpGet]
        public async Task<IActionResult> GetRandevuSaatleriJson(DateTime secilenTarih, int doktorId)
        {
            var saatler = await GetRandevuSaatleri(secilenTarih, doktorId);
            return Json(new SelectList(saatler, "Value", "Text"));
        }
        private async Task<List<SelectListItem>> GetRandevuSaatleriEdit(DateTime randevuTarihi, int randevuId)
        {
            var mevcutRandevu = await dbcontext.Randevular.FindAsync(randevuId);
            var mevcutRandevuSaati = mevcutRandevu?.RandevuSaati;

            var alinanSaatler = await dbcontext.Randevular
                .Where(r => r.RandevuTarih.Date == randevuTarihi.Date && r.RandevuId != randevuId)
                .Select(r => r.RandevuSaati)
                .ToListAsync();

            var saatler = new List<SelectListItem>();
            var baslangic = new TimeSpan(9, 0, 0);
            var bitis = new TimeSpan(17, 0, 0);
            var aralik = TimeSpan.FromMinutes(30);

            for (var saat = baslangic; saat < bitis; saat += aralik)
            {
                if (!alinanSaatler.Contains(saat) || saat == mevcutRandevuSaati)
                {
                    saatler.Add(new SelectListItem
                    {
                        Value = saat.ToString(@"hh\:mm"),
                        Text = saat.ToString(@"hh\:mm"),
                        Selected = saat == mevcutRandevuSaati
                    });
                }
            }

            return saatler;
        }
        private async Task<List<SelectListItem>> GetRandevuSaatleri(DateTime randevuTarihi, int doktorId)
        {

            var alinanSaatler = await dbcontext.Randevular
                .Where(r => r.RandevuTarih.Date == randevuTarihi.Date && r.DoktorId == doktorId)
                .Select(r => r.RandevuSaati)
                .ToListAsync();

            var saatler = new List<SelectListItem>();
            var baslangic = new TimeSpan(9, 0, 0);
            var bitis = new TimeSpan(17, 0, 0);
            var aralik = TimeSpan.FromMinutes(30);

            for (var saat = baslangic; saat < bitis; saat += aralik)
            {
                if (!alinanSaatler.Contains(saat))
                {
                    saatler.Add(new SelectListItem
                    {
                        Value = saat.ToString(),
                        Text = saat.ToString(@"hh\:mm")
                    });
                }
            }

            return saatler;
        }

        // private async Task<List<SelectListItem>> GetRandevuSaatleri(DateTime? secilenTarih)
        // {
        //     var mevcutRandevular = new HashSet<TimeSpan>();

        //     if (secilenTarih.HasValue)
        //     {
        //         var randevuTarihleri = await dbcontext.Randevular
        //             .Where(r => r.RandevuTarih.Date == secilenTarih.Value.Date)
        //             .Select(r => r.RandevuTarih.TimeOfDay)
        //             .ToListAsync();

        //         mevcutRandevular = new HashSet<TimeSpan>(randevuTarihleri);
        //     }

        //     var saatler = new List<SelectListItem>();
        //     var baslangic = new TimeSpan(9, 0, 0); // Sabah 9:00
        //     var bitis = new TimeSpan(17, 0, 0);    // Akşam 5:00

        //     for (var saat = baslangic; saat < bitis; saat = saat.Add(TimeSpan.FromMinutes(30)))
        //     {
        //         if (!mevcutRandevular.Contains(saat))
        //         {
        //             saatler.Add(new SelectListItem { Value = saat.ToString(), Text = saat.ToString(@"hh\:mm") });
        //         }
        //     }

        //     return saatler;
        // }
        [HttpPost]
        public async Task<IActionResult> RandevuOlustur(Randevu model)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin" || userRole != "Hasta")
            {
                return Unauthorized();
            }
            dbcontext.Randevular.Add(model);
            await dbcontext.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        // [HttpGet]
        // public IActionResult Edit(int id)
        // {
        //     var randevu = dbcontext.Randevular.SingleOrDefault(r => r.RandevuId == id);
        //     if (randevu == null)
        //     {
        //         return NotFound();
        //     }

        //     var viewModel = new RandevuEditViewModel
        //     {
        //         RandevuId = randevu.RandevuId,
        //         HastaId = randevu.HastaId,
        //         DoktorId = randevu.DoktorId,
        //         RandevuTarih = randevu.RandevuTarih,
        //         RandevuSaati = randevu.RandevuSaati
        //     };

        //     ViewBag.Hastalar = new SelectList(dbcontext.Hastalar, "HastaId", "HastaAd", randevu.HastaId);
        //     ViewBag.Doktorlar = new SelectList(dbcontext.Doktorlar, "DoktorId", "DoktorAd", randevu.DoktorId);
        //     return View(viewModel);
        // }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin" || userRole != "Hasta")
            {
                return Unauthorized();
            }
            var loginName = HttpContext.Session.GetString("LoginName");


            var randevu = await dbcontext.Randevular
                .Include(r => r.Hasta)
                .Include(r => r.Doktor)
                .SingleOrDefaultAsync(r => r.RandevuId == id);
            if (userRole == "Hasta")
            {
                var hasta = await dbcontext.Hastalar.FirstOrDefaultAsync(h => h.HastaTc == loginName);
                if (hasta != null)
                {
                    ViewBag.Hastalar = new SelectList(new List<Hasta> { hasta }, "HastaId", "HastaAd", randevu.RandevuId);

                }
            }
            if (randevu == null)
            {
                return NotFound();
            }

            var viewModel = new RandevuEditViewModel
            {
                RandevuId = randevu.RandevuId,
                HastaId = randevu.HastaId,
                DoktorId = randevu.DoktorId,
                RandevuTarih = randevu.RandevuTarih,
                RandevuSaati = randevu.RandevuSaati
            };

            //ViewBag.Hastalar = new SelectList(dbcontext.Hastalar, "HastaId", "HastaAd", randevu.HastaId);
            ViewBag.Doktorlar = new SelectList(dbcontext.Doktorlar, "DoktorId", "DoktorAd", randevu.DoktorId);
            ViewBag.RandevuSaatleri = await GetRandevuSaatleriEdit(randevu.RandevuTarih, randevu.RandevuId);

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Edit(int id, RandevuEditViewModel viewModel)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin" || userRole != "Hasta")
            {
                return Unauthorized();
            }
            if (id != viewModel.RandevuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var randevuToUpdate = dbcontext.Randevular.Find(id);
                if (randevuToUpdate == null)
                {
                    return NotFound();
                }

                randevuToUpdate.HastaId = viewModel.HastaId;
                randevuToUpdate.DoktorId = viewModel.DoktorId;
                randevuToUpdate.RandevuTarih = viewModel.RandevuTarih;
                randevuToUpdate.RandevuSaati = viewModel.RandevuSaati;

                dbcontext.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Hastalar = new SelectList(dbcontext.Hastalar, "HastaId", "HastaAd", viewModel.HastaId);
            ViewBag.Doktorlar = new SelectList(dbcontext.Doktorlar, "DoktorId", "DoktorAd", viewModel.DoktorId);

            return View(viewModel);
        }
        public IActionResult Delete(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin" || userRole != "Hasta")
            {
                return Unauthorized();
            }
            var randevu = dbcontext.Randevular.Find(id);
            if (randevu != null)
            {
                dbcontext.Randevular.Remove(randevu);
                dbcontext.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
    }



}