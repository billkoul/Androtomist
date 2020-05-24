using System.Collections.Generic;
using Androtomist.Models.Forms;

namespace Androtomist.Models.Database.Datatables
{
    public class DropDownRequest
    {
        public string q { get; set; }
        public int page { get; set; }
        public string column_name { get; set; }
        public string json { get; set; }
        public TABLE_TYPE tabletype { get; set; }

        public List<DropDownColumns.FilteredColumn> GetFilteredColumns()
        {
            DropDownColumns dropDownColumns = new DropDownColumns(json);
            return dropDownColumns.GetFilteredColumns();
        }

    }
}
