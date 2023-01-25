using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace TerritoryDirectory.Controllers;

public class AccountController : Controller
{
    // GET: /Account/
    public async Task<IActionResult> Index()
    {
        return RedirectToAction(nameof(Login));
    }

    // GET: /Account/Login/
    public async Task<IActionResult> Login()
    {
        return View();
    }
}