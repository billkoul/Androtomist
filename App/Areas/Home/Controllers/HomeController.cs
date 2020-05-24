using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Androtomist.Models.Forms;


namespace Androtomist.Controllers
{
    [Area("Home"), Authorize]
    public class HomeController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                string routeWhereExceptionOccurred = exceptionFeature.Path;
                Exception exceptionThatOccurred = exceptionFeature.Error;
            }

            return View();
        }

        public IActionResult getStatus()
        {
            FormResponse formResponse = new FormResponse();

            try
            {
                formResponse.result = 1;
                formResponse.msg = "ok";
                formResponse.data = 1;
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
