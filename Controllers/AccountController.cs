using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace TerritoryDirectory.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{

    static HttpClient client = new HttpClient();

    // GET: /Account/
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Login));
    }

    // GET: /Account/Login/
    public IActionResult Login()
    {
        return View();
    }

    // POST: /Account/Login/
    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var credentials = new {username, password};
        var content = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://netzwelt-devtest.azurewebsites.net/Account/SignIn", content);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            var userName = data.username;
            var roles = data.roles;

            await SignInUser(username, JsonConvert.SerializeObject(roles));

            //redirect to the home page
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        else
        {
            // show an error message on the login page and allow the user to try again
            ModelState.AddModelError("", "Invalid Login Attempt");
            return View();
        }
    }

    private async Task SignInUser(string username, string roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim("Role", roles)
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));
    }
}