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

namespace Androtomist.Areas.Uploads.Controllers
{
	[Area("Files"), Authorize]
	[AutoValidateAntiforgeryToken]
	public class FilesController : Controller
	{

		public IActionResult Index()
		{
			UploadsDatatable abstractDatatable = new UploadsDatatable(new DataTableRequest());

			return View(abstractDatatable);
		}

		public IActionResult Add(string id)
		{
			Submission submission;

			if (id == "" || id == null)
			{
				SubmissionInserter submissionInserter = new SubmissionInserter
				{
					SUB_ID = null,
					SUBS_SCHEMA_ID = "0",
					SUBS_LOG = ""
				};
				long SUB_ID = submissionInserter.Insert(true);
				submission = new Submission(SUB_ID);

				RedirectToActionResult redirectResult = new RedirectToActionResult("add", "files", new { @id = SUB_ID });
				return redirectResult;
			}
			else
			{
				long.TryParse(id, out long SUB_ID);
				submission = new Submission(SUB_ID);
			}

			return View(submission);
		}

		public IActionResult NewUpload(string formJson)
		{
			FormResponse formResponse = new FormResponse();
			FilterData filterData = new FilterData(formJson);

			try
			{

				FileInserter fileInserter = new FileInserter
				{
					FILE_ID = filterData.UPLOAD_ID,
					FILE_LABEL = filterData.FILE_LABEL
				};
				long ID = fileInserter.Insert();

				formResponse.msg = "File saved succesfully";
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

		[IgnoreAntiforgeryToken]
		public IActionResult UploadFiles(IFormFile File, string SUB_ID)
		{
			FormResponse formResponse = new FormResponse();
			string upload_path = string.Empty;
			FileHelper fileHelper = new FileHelper();

			string randomfolder = Path.GetRandomFileName();

			if (File != null && File.Length > 0)
			{
				try
				{
					using (var fileStream = new FileStream(Path.Combine(fileHelper.GetUploadPath(true, randomfolder), File.FileName), FileMode.Create))
					{
						File.CopyTo(fileStream);
						upload_path = fileStream.Name;
					}

					FileInserter fileInserter = new FileInserter
					{
						FILE_ID = null,
						FILE_NAME = File.FileName,
						FILE_SIZE = File.Length.ToString(),
						FILE_PATH = upload_path,
						FILE_RELATIVE_PATH = upload_path.Substring(upload_path.IndexOf("wwwroot")+7),
					};
					long ID = fileInserter.Insert(true);

					SubmissionFileInserter submissionFileInserter = new SubmissionFileInserter
					{
						SUBFILE_SUB_ID = SUB_ID,
						SUBFILE_FILE_ID = ID.ToString()
					};
					long SUB_FILE_ID = submissionFileInserter.Insert(true);

					formResponse.msg = "File saved succesfully"; //don't need this yet
					return Json(ID);
				}
				catch (FormDataException ex)
				{
					formResponse.msg = ex.ToString(); //don't need this yet
				}
				catch (Exception ex)
				{
					formResponse.msg = ex.ToString(); //don't need this yet
				}
			}
			
			return Json(0);
		}

		public IActionResult DeleteUpload(string id)
		{
			FormResponse formResponse = new FormResponse();

			try
			{
				FileHelper fileHelper = new FileHelper();
				fileHelper.RemoveFiles(Convert.ToInt64(id));

				formResponse.data = "File has been removed.";
				formResponse.msg = "File has been removed.";
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