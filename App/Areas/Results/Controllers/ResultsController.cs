using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Androtomist.Models.Database.Datatables;
using Androtomist.Models.Database.Entities;
using Androtomist.Models.Forms;
using Androtomist.Models.Database.Helpers;

namespace Androtomist.Areas.Results.Controllers
{
	[Area("Results"), Authorize]
	public class ResultsController : Controller
	{

		public IActionResult Index()
		{
            ResultsDatatable abstractDatatable = new ResultsDatatable(new DataTableRequest());

            return View(abstractDatatable);
		}

        public IActionResult Show(string id)
        {
            Result result;

            if (id == "" || id == null)
            {
                long.TryParse(id, out long P_ID);
                result = new Result(P_ID);
            }
            else
            {
                return Redirect("/results/results/index");
            }

            return View(result);
        }

        public IActionResult File(string id)
        {
            ResultInfo info;

            if (id == "" || id == null)
            {
                return Redirect("/results/results/index");
            }
            else
            {
                long.TryParse(id, out long F_ID);
                info = new ResultInfo(F_ID);
            }

            return View(info);
        }

        public IActionResult Export(string fileId)
        {
            FormResponse formResponse = new FormResponse();
            ResultsHelper rh = new ResultsHelper();

            try
            {
                long.TryParse(fileId, out long fId);
                string link = rh.Export(fId);
                formResponse.msg = link;
            }
            catch
            {
                formResponse.result = 0;
                formResponse.msg = "Error while exporting file";
            } 

            return Json(formResponse);
        }
    }
}