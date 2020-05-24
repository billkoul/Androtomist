using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Androtomist.Models.Forms;
using Androtomist.Models.Processing;

namespace Androtomist.Controllers
{
    [Area("Train"), Authorize]
    public class TrainController : Controller
    {
        public IActionResult Index()
        {
            GeneralPriviledgesCheker p = new GeneralPriviledgesCheker();
            if (!p.IsPowerfull())
                return RedirectToAction("Index", "Home", new { area = "Home" });

            return View();
        }

        public IActionResult DoTrain(string goodware, string malware)
        {
            FormResponse formResponse = new FormResponse();

            try
            {
                GeneralPriviledgesCheker p = new GeneralPriviledgesCheker();
                if (!p.IsPowerfull())
                    return RedirectToAction("Index", "Home", new { area = "Home" });

                Trainer trainer = new Trainer(goodware, malware);
                
                formResponse.msg = "Training data created!";
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
