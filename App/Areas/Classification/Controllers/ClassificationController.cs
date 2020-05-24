using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Androtomist.Models.Database.Helpers;
using Androtomist.Models.Forms;

namespace Androtomist.Controllers
{
    [Area("Classification"), Authorize]
    public class ClassificationController : Controller
    {
        public IActionResult Index()
        {
            GeneralPriviledgesCheker p = new GeneralPriviledgesCheker();
            if (!p.IsPowerfull())
                return RedirectToAction("Index", "Home", new { area = "Home" });

            return View();
        }

        public IActionResult CreateVectors()
        {
            FormResponse formResponse = new FormResponse();

            try
            {
                GeneralPriviledgesCheker p = new GeneralPriviledgesCheker();
                if (!p.IsPowerfull())
                    return RedirectToAction("Index", "Home", new { area = "Home" });

                ClassificationHelper ch = new ClassificationHelper();
                
                formResponse.data = ch.CreateVectors();
                formResponse.msg = "Vectors created";
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
