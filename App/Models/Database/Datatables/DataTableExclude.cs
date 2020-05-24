using System.Collections.Generic;

namespace Androtomist.Models.Database.Datatables
{
    public class DataTableExclude
    {
        public long UNIQUE_ID { get; set; }
        public List<string> KEYS { get; set; }
        public TABLE_TYPE TABLE_TYPE { get; set; }

        public DataTableExclude()
        {
            UNIQUE_ID = 0;
            KEYS = new List<string>();
            TABLE_TYPE = TABLE_TYPE.NONE;
        }
    }
}
