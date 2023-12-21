using Microsoft.AspNetCore.Mvc;
using ASPWebProgramming.Data;

public class HastaController : Controller
{
    private readonly DataContext _context;

    public HastaController(DataContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var hastalar = _context.Hastalar.ToList();
        return View(hastalar);
    }

[HttpGet]
public IActionResult Edit(int id)
{
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
    if (id != hasta.HastaId)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        hasta.HastaSifre = $"{hasta.HastaTc.Substring(0, 3)}{hasta.HastaTel.Substring(hasta.HastaTel.Length - 3)}";
        _context.Update(hasta);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    return View(hasta);
}
public IActionResult Delete(int id)
{
    var hasta = _context.Hastalar.Find(id);
    if (hasta != null)
    {
        _context.Hastalar.Remove(hasta);
        _context.SaveChanges();
    }
    return RedirectToAction(nameof(Index));
}
}
