using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspWebProgram.Models;
using Microsoft.AspNetCore.Localization;
using AspWebProgram.Services;

namespace AspWebProgram.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private  LanguageServices _localization;
    public HomeController(ILogger<HomeController> logger,LanguageServices localization)
    {
        _logger = logger;
        _localization=localization;
    }

    public IActionResult Index()
    {
        ViewBag.Welcome=_localization.GetKey("Welcome").Value;
        var currentCulture=Thread.CurrentThread.CurrentCulture.Name;
        return View();
    }
    public IActionResult ChangeLanguage(string culture)
    {
        Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),new CookieOptions()
        {
            Expires=DateTimeOffset.UtcNow.AddYears(1)
        });
        return Redirect(Request.Headers["Referer"].ToString());
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
