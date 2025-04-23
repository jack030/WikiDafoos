using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WikiDafoos.Models;

namespace WikiDafoos.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
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
        ViewData["Title"] = "������ ��";
        return View();
    }

    public IActionResult Contact()
    {
        //ViewBag.Title = "Hello";
        ViewData["Title"] = "���� �� ��";

        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
