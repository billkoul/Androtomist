using Microsoft.AspNetCore.Mvc;
using System;
using Androtomist.Models.Database.Entities;

namespace Androtomist.Areas.Account.Controllers
{
    [Area("Account")]
    [AutoValidateAntiforgeryToken]

    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            Models.User.UserClaims claims = new Models.User.UserClaims();
            long USER_ID;

            USER_ID = Convert.ToInt64(claims.GetSpecificClaim(System.Web.HttpContext.Current.User.Identity, System.Security.Claims.ClaimTypes.PrimarySid));

            User user = new User(USER_ID);
            ViewData["user_level"] = user.USER_LEVEL;
            ViewData["is_me"] = "yes";

            return View("~/Areas/Dashboard/Views/Users/Add.cshtml", model: user);
        }
    }
}