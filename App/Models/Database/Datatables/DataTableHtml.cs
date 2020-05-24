namespace Androtomist.Models.Database.Datatables
{
    public class DataTableHtml
    {
        public long UNIQUE_ID;
        public TABLE_TYPE TABLE_TYPE;
        public string TITLE { get; set; }
        public string ENTITY_NAME { get; set; }
        public string URL_ADD { get; set; }
        public string EXTRA_FILTER { get; set; }
        public int DEFAULT_ORDER_COL { get; set; }
        public string DEFAULT_ORDER_DIR { get; set; }
        public bool CAN_REMOVE { get; set; }
        public bool CAN_MULTI_REMOVE { get; set; }
        public bool CAN_CLONE { get; set; }
        public DataColumn[] COLUMNS { get; set; }

        public DataTableHtml()
        {
            UNIQUE_ID = 0;
            TABLE_TYPE = TABLE_TYPE.NONE;
            TITLE = string.Empty;
            ENTITY_NAME = string.Empty;
            URL_ADD = string.Empty;
            EXTRA_FILTER = "0";
            DEFAULT_ORDER_COL = 1;
            DEFAULT_ORDER_DIR = "asc";
            CAN_REMOVE = true;
            CAN_MULTI_REMOVE = true;
            CAN_CLONE = false;
            COLUMNS = new DataColumn[] { };
        }

        public class DataColumn
        {
            public string NAME { get; set; }
            public string TITLE { get; set; }
            public bool SEARCHABLE { get; set; }
            public bool ORDERABLE { get; set; }


            public DataColumn(string NAME, string TITLE)
            {
                this.NAME = NAME;
                this.TITLE = TITLE;
                this.SEARCHABLE = true;
                this.ORDERABLE = true;
            }

            public DataColumn(string NAME, string TITLE, bool SEARCHABLE, bool ORDERABLE)
            {
                this.NAME = NAME;
                this.TITLE = TITLE;
                this.SEARCHABLE = SEARCHABLE;
                this.ORDERABLE = ORDERABLE;
            }



        }
    }
}
