using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Androtomist.Models.Database.Datatables;
using Androtomist.Models.Database.Entities;
using Androtomist.Models.Database.Helpers;
using Androtomist.Models.Forms;

namespace Androtomist.Areas.Dashboard.Controllers
{
    [Area("Dashboard"), Authorize]
    [AutoValidateAntiforgeryToken]

    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            GeneralPriviledgesCheker p = new GeneralPriviledgesCheker();
            if (!p.IsPowerfull())
                return RedirectToAction("Index", "Home", new { area = "Home" });

            UsersDatatable abstractDatatable = new UsersDatatable(new DataTableRequest());

            return View(abstractDatatable);
        }

        public IActionResult Add(string id)
        {
            Models.User.UserClaims claims = new Models.User.UserClaims();
            User userCurrent, user;

            if (id == "" || id == null)
                user = new User(-1);
            else
            {
                long.TryParse(id, out long USER_ID);
                user = new User(USER_ID);
            }

            long userId = Convert.ToInt64(claims.GetSpecificClaim(System.Web.HttpContext.Current.User.Identity, System.Security.Claims.ClaimTypes.PrimarySid));
            userCurrent = new User(userId);

            GeneralPriviledgesCheker p = new GeneralPriviledgesCheker();
            if (!p.IsPowerfull() && userId != userCurrent.USER_ID)
                return RedirectToAction("Index", "Home", new { area = "Home" });

            ViewData["user_level"] = userCurrent.USER_LEVEL;
            ViewData["is_me"] = user.Exists() && user.USER_ID.Equals(userCurrent.USER_ID) ? "yes" : "no";

            return View(user);
        }

        public IActionResult NewUser(string formJson)
        {
            GeneralPriviledgesCheker p = new GeneralPriviledgesCheker();
            if (!p.IsPowerfull())
                return RedirectToAction("Index", "Home", new { area = "Home" });

            FormResponse formResponse = new FormResponse();

            try
            {
                UserHelper userhelper = new UserHelper();
                User user = userhelper.AddUser(formJson);

                if (!user.HAS_CHANGES)
                    formResponse.msg = "User created succesfully";
                else
                    formResponse.msg = "User updated succesfully";

                formResponse.data = user.USER_ID;
            }
            catch (FormDataException ex)
            {
                formResponse.result = 0;
                formResponse.msg = ex.Message;
            }
            catch (Exception ex)
            {
                formResponse.result = 0;
                formResponse.msg = ex.Message;
            }

            return Json(formResponse);
        }
    }
}