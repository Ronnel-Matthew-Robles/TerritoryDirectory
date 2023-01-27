using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TerritoryDirectory.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TerritoryDirectory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace TerritoryDirectory.Controllers;

[Authorize]
public class HomeController : Controller
{
    static HttpClient client = new HttpClient();

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var response = await client.GetAsync("https://netzwelt-devtest.azurewebsites.net/Territories/All");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            ViewData["territories"] = json;

            return View();
        }
        else
        {
            Console.WriteLine("Unable to fetch from API");
            return View();
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
