using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Androtomist.Areas.Account.Controllers
{
    [Area("Account")]
    [AutoValidateAntiforgeryToken]

    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("index", "login", new { area = "account" });
        }
    }
}