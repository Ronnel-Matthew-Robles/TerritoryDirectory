﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TerritoryDirectory.Models;

namespace TerritoryDirectory.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        HttpContext.Session.SetString("UserName", "matt");
        if (HttpContext.Session.GetString("UserName") == null)
        {
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
