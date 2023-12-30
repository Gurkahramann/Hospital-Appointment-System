using Microsoft.AspNetCore.Mvc;
using AspWebProgramming.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

public class HastaController : Controller
{
    private readonly DataContext _context;

    public HastaController(DataContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        var loginName = HttpContext.Session.GetString("LoginName");

        if (userRole == "Hasta")
        {
            // Sadece giriş yapan hasta kullanıcısının bilgilerini getir
            var hastalar = await _context.Hastalar
                .Where(h => h.HastaTc == loginName)
                .ToListAsync();
            return View(hastalar);
        }
        else if (userRole == "Doktor")
        {
            // Sadece giriş yapan doktordan randevu alan hastaların bilgilerini getir
            var hastalar = await _context.Randevular
                .Include(r => r.Hasta) // Randevu ile ilişkili Hasta bilgilerini içer
                .Where(r => r.Doktor.DoktorTc == loginName)
                .Select(r => r.Hasta)
                .Distinct() // Aynı hastanın birden fazla randevusu varsa, hastayı bir kere listele
                .ToListAsync();
            return View(hastalar);
        }
        else
        {
            // Diğer roller için tüm hastaları getir
            var hastalar = await _context.Hastalar.ToListAsync();
            return View(hastalar);
        }
    }

    // public IActionResult Index()
    // {
    //     var userRole = HttpContext.Session.GetString("UserRole");
    //     var loginName = HttpContext.Session.GetString("LoginName");

    // List<Hasta> hastalar;
    // if (userRole == "Hasta")
    // {
    //     // Sadece giriş yapan hasta kullanıcısının bilgilerini getir
    //      hastalar = _context.Hastalar.Where(h => h.HastaTc == loginName).ToList();
    // }
    // else
    // {
    //     // Diğer roller için tüm hastaları getir
    //     hastalar = _context.Hastalar.ToList();
    // }
    // return View(hastalar);
    // }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        if (userRole != "Admin" || userRole != "Hasta")
        {
            return Unauthorized();
        }
        var hasta = _context.Hastalar.Find(id);
        if (hasta == null)
        {
            return NotFound();
        }
        return View(hasta);
    }
    [HttpPost]
    public IActionResult Edit(int id, Hasta hasta)
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        if (userRole != "Admin" || userRole != "Hasta")
        {
            return Unauthorized();
        }
        if (id != hasta.HastaId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            hasta.Rol = "Hasta";
            _context.Update(hasta);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(hasta);
    }
    public IActionResult Delete(int id)
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        if (userRole != "Admin" || userRole != "Hasta")
        {
            return Unauthorized();
        }
        var hasta = _context.Hastalar.Find(id);
        if (hasta != null)
        {
            _context.Hastalar.Remove(hasta);
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }
}
