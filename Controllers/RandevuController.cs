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
            return RedirectToAction("Index","Home");
        }

    }



}