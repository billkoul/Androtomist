using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Androtomist.Models.Database.Datatables;

namespace Androtomist.ViewComponents
{
    [ViewComponent(Name = "DataTableComponent")]
    public class DataTableComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(AbstractDatatable abstractDatatable)
        {
            return View(await Task.FromResult(abstractDatatable.GetDataTableHtml()));
        }

    }
}