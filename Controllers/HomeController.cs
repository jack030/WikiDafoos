using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WikiDafoos.Models;
using WikiDafoos.Models.ViewModel;

namespace WikiDafoos.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DafoosDbContext _dafoosDbContext;
    public HomeController(ILogger<HomeController> logger, DafoosDbContext dafoosDbContext)
    {
        _logger = logger;
        _dafoosDbContext = dafoosDbContext;
    }

    public async Task<IActionResult> Index()
    {
        var mostViewedArticles = await _dafoosDbContext.Articles.Include(x => x.Category)
            .OrderByDescending(x => x.Views)
            .Take(10)
            .ToListAsync();
        var latestArticles = await _dafoosDbContext.Articles.Include(x => x.Category)
           .OrderByDescending(x => x.CreationTime)
           .Take(10)
           .ToListAsync();
        var suggestedArticles = await _dafoosDbContext.Articles.Include(x => x.Category).Where(x => x.IsSuggested == true)
            .OrderByDescending(x => x.CreationTime)
            .Take(10)
            .ToListAsync();
        var viewModel = new HomeIndexViewModel
        {
            LatestArticles = latestArticles,
            SuggestedArticles = suggestedArticles,
            TopViewArticles = mostViewedArticles
        };
        return View(viewModel);
    }

    public IActionResult Dashboard()
    {
        return View();
    }

    //public IActionResult Articles()
    //{
    //    return View();
    //}
    public IActionResult About()
    {
        ViewData["Title"] = "œ—»«—Â „«";
        return View();
    }

    public IActionResult Contact()
    {
        //ViewBag.Title = "Hello";
        ViewData["Title"] = " „«” »« „«";

        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
