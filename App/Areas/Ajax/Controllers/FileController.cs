using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Androtomist.Models.Database.Entities;

namespace Androtomist.Areas.Ajax.Controllers
{
    [Area("Ajax"), Authorize]
    public class FileController : Controller
    {
        public IActionResult Index()
        {
            return NoContent();
        }

        public IActionResult Download(string id, bool isinput = false)
        {
            if (!isinput)
            {
                long.TryParse(id, out long FILE_ID);

                File file = new File(FILE_ID);

                return Redirect(file.DOWNLOAD_PATH);
            }
            else
            {
                long.TryParse(id, out long INPUT_ID);

                File file = new File(INPUT_ID);

                //return Redirect(baselineInput.DOWNLOAD_EXAMPLE_PATH);
                return Redirect("");
            }
        }
    }

}