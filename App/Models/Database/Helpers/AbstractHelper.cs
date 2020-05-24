using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Androtomist.Models.Database.Helpers
{
    public abstract class AbstractHelper : DBClass
    {
        public AbstractHelper()
        {

        }

        protected long CloneRow(string tableName, string colName, long idCopy, long idPaste)
        {
            DataTable dataTable;
            string SQL;
            List<string> columns;

            dataTable = databaseConnector.SelectSQL("SELECT * FROM " + tableName + " WHERE 1=0", tableName);

            columns = dataTable.Columns.Cast<DataColumn>().Where(x => !x.ColumnName.Equals(colName)).Select(x => x.ColumnName).ToList();

            SQL = "INSERT INTO " + tableName + "(" + colName + ", " + string.Join(", ", columns) + ") ";

            SQL += "SELECT " + idPaste + " AS " + colName + ", " + string.Join(", ", columns) + " FROM " + tableName + " WHERE " + colName + " = " + idCopy;

            // Delete in case default values are loaded on creation
            databaseConnector.DeleteSQL("DELETE FROM " + tableName + " WHERE " + colName + " = " + idPaste);

            return databaseConnector.InsertSQL(SQL);
        }

        protected class JsonVals
        {
            public string name { get; set; }
            public string value { get; set; }
        }
    }
}
