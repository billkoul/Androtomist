using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Androtomist.Models.Database.Datatables;
using Androtomist.Models.Database.Entities;
using Androtomist.Models.Database.Helpers;
using Androtomist.Models.Database.Inserters;
using Androtomist.Models.Forms;
using System.IO;
using Androtomist.Models.Processing;

namespace Androtomist.Areas.Analysis.Controllers
{
	[Area("Analysis"), Authorize]
	[IgnoreAntiforgeryToken]

	public class AnalysisController : Controller
	{

		public IActionResult Index()
		{
            ProcessDatatable abstractDatatable = new ProcessDatatable(new DataTableRequest());

			return View(abstractDatatable);
		}

		public IActionResult Add(string id)
		{
            Process process;

			if (id == "" || id == null)
			{
                process = new Process(-1);
            }
			else
			{

				GeneralPriviledgesCheker p = new GeneralPriviledgesCheker();
				if (!p.IsPowerfull())
					return RedirectToAction("Index", "Analysis", new { area = "Analysis" });

				long.TryParse(id, out long P_ID);
                process = new Process(P_ID);
			}

			return View(process);
		}

        public IActionResult File(string id)
        {
            Process process;

            if (id == "" || id == null)
            {
                process = new Process(-1);
            }
            else
            {
                long.TryParse(id, out long F_ID);
                process = new Process(-1);
                ViewData["file_id"] = F_ID;
             }

            return View(process);
        }

        public IActionResult NewAnalysis(string formJson)
		{
			FormResponse formResponse = new FormResponse();

			try
			{
                ProcessHelper processHelper = new ProcessHelper();
                Process process = processHelper.AddProcess(formJson);

                Analyzer proc = new Analyzer(process);
				long pId = proc.Analyze();

				if (proc.isSuccesful)
                    formResponse.msg = pId.ToString();
                else
                    formResponse.msg = "Process failed";
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