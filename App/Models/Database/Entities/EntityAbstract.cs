using System;
using System.Data;

namespace Androtomist.Models.Database.Entities
{
    public abstract class EntityAbstract : DBClass
    {
        protected DataRow dataRow;
        public DataTable dataTable;

        public EntityAbstract()
        {
            dataRow = null;
        }

        public void GetRow(string SQL)
        {
            DataTable dataTable;

            dataTable = databaseConnector.SelectSQL(SQL, "TABLE");

            if (dataTable.Rows.Count > 0)
                dataRow = dataTable.Rows[0];
        }

        public void GetTable(string SQL)
        {
            dataTable = databaseConnector.SelectSQL(SQL, "TABLE");
        }

        public bool Exists()
        {
            return dataRow != null;
        }

       
    }
}
