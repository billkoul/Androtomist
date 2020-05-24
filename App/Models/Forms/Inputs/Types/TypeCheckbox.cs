using System;
using System.Data;
using Androtomist.Models.Forms;

namespace Androtomist.Models.Database.Inputs
{
    public class TypeCheckbox : TypeAbstract
    {
		public TypeCheckbox(string tableName = "", string column = "", string name = "", string current = "", string fitlerWith = "", Dependable d = null, bool is_parent = false) : base()
        {
			this.name = name;
			this.column = column;
			this.tableName = tableName;
			this.fitlerWith = fitlerWith;
			this.current = current;
			this.d = d;
			this.is_parent = is_parent;

			Populate("SELECT * FROM " + tableName, tableName);
		}

        public override void Populate(string SQL, string tableName)
        {
			DataTable dt = databaseConnector.SelectSQL(SQL, tableName);

			foreach (DataRow row in dt.Rows)
			{
				html += "<br>" + row[column] + " <input type='radio' id='" + name + "' name='" + name + "' value='" + row[0] + "' />";
			}
        }
    }
}
