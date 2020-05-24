using System.Collections.Generic;
using Androtomist.Models.Forms;

namespace Androtomist.Models.Database.Datatables
{
    public class DataTableRequest
    {
        public int draw { get; set; }
        public int sEcho { get; set; }

        public DtColumn[] columns { get; set; }

        public DtOrder[] order { get; set; }

        public int start { get; set; }
        public int length { get; set; }

        public DtSearch search { get; set; }

        public string[] columnsDef { get; set; }
        public string json { get; set; }

        public TABLE_TYPE tabletype { get; set; }

        public List<DropDownColumns.FilteredColumn> GetFilteredColumns()
        {
            DropDownColumns dropDownColumns = new DropDownColumns(json);
            return dropDownColumns.GetFilteredColumns();
        }

    }

    public class DtSearch
    {
        public string value { get; set; }

        //[JsonConverter(typeof(BoolConverter))]
        public string regex { get; set; }
    }

    public class DtOrder
    {
        public int column { get; set; }
        public string dir { get; set; }
    }

    public class DtColumn
    {
        public string data { get; set; }
        public string name { get; set; }

        //[JsonConverter(typeof(BoolConverter))]
        public string searchable { get; set; }

        //[JsonConverter(typeof(BoolConverter))]
        public string orderable { get; set; }

        public DtSearch search { get; set; }
    }
}
