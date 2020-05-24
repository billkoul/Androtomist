using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Androtomist.Models.Database.Entities;
using Androtomist.Models.Forms;

namespace Androtomist.Areas.Ajax.Controllers
{
    [Area("Ajax"), Authorize]
    public class SelectController : Controller
    {
        public IActionResult Index()
        {
            return NoContent();
        }

        public IActionResult GetValues(string id)
        {
            return Ok();
        }
    }
}