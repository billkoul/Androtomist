using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Androtomist.Models.Components.Login;
using Androtomist.Models.Database.User;
using Androtomist.Models.Forms;
//using Androtomist.Models.Statics;

namespace Androtomist.Areas.Account.Controllers
{
    [Area("Account")]
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            string ReturnUrl = HttpContext.Request.Query["ReturnUrl"].ToString();
            if (string.IsNullOrWhiteSpace(ReturnUrl)) ReturnUrl = "/";
            ViewData["ReturnUrl"] = ReturnUrl;

            if (User.Identity.IsAuthenticated)
                return RedirectToAction("index", "account", new { area = "account" });
            else
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModel loginModel)
        {
            FormResponse formResponse = new FormResponse();

            try
            {
                UserLoginValidator userLoginValidator = new UserLoginValidator(loginModel.Username, loginModel.Password);

                if (userLoginValidator.CheckCredentials())
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.PrimarySid, userLoginValidator.UserID.ToString()),
                    new Claim(ClaimTypes.Name, loginModel.Username),
                    new Claim(ClaimTypes.GivenName, userLoginValidator.Name),
                    new Claim(ClaimTypes.Surname, userLoginValidator.SurName)
                };

                    var userIdentity = new ClaimsIdentity(claims, "login");

                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                    await HttpContext.SignInAsync(principal, new AuthenticationProperties
                    {
                        IsPersistent = true,
                        AllowRefresh = true,
                        ExpiresUtc = DateTime.UtcNow.AddDays(60)
                    });

                    formResponse.msg = "Successful login.<br />Please wait redirecting...";
                }
                else
                {
                    formResponse.result = 0;
                    formResponse.msg = "Wrong username or password.<br />Please try again.";
                }
            }
            catch (Exception ex)
            {
                formResponse.result = 0;
                formResponse.msg = ex.Message;
            }

            return Json(formResponse);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Antiforgery()
        {
            return Content("Successful antiforgery!");
        }


    }
}