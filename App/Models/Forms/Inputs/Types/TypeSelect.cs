using System;
using System.Data;
using System.Collections.Generic;
using Androtomist.Models.Forms;


namespace Androtomist.Models.Database.Inputs
{
    public class TypeSelect : TypeAbstract
    {
        public TypeSelect(string tableName = "", string column = "", string name = "", string current = "", string fitlerWith = "", Dependable d = null, bool is_parent = false, string rewrite = "") : base()
        {
            this.name = name;
            this.column = column;
            this.tableName = tableName;
            this.fitlerWith = fitlerWith;
            this.current = current;
			this.d = d;
			this.is_parent = is_parent;

			string sql = (!string.IsNullOrEmpty(rewrite) ? rewrite : "SELECT * FROM " + tableName);
			Populate(sql, tableName);
        }

        public override void Populate(string SQL, string tableName)
        {
            DataTable dt = databaseConnector.SelectSQL(SQL, tableName);

			string extra_class = "";
			if (d != null)
			{
				foreach(string[,] input in d.inputs)
				{
					extra_class += " depend_" + input[0, 0] + "_" + input[1, 0];
				}
			}
			extra_class += (is_parent ? " parent" : ""); 

			html = "<select class='form-control m-input" + extra_class + "' id='" + name + "' name='" + name + "' " + (dt.Rows[0].Table.Columns.Contains("DATA") ? "data="+ dt.Rows[0]["DATA"] : "") + ">";

            foreach (DataRow row in dt.Rows)
            {
                html += "<option value = '" + row[0] + "'" + (!(string.IsNullOrEmpty(fitlerWith)) ? "data='" + row[fitlerWith] + "'" : "") + (!(string.IsNullOrEmpty(fitlerWith)) ? " style='display: none;'" : "") + ((!string.IsNullOrEmpty(current) && row[0].ToString() == current) ? " selected" : "") + ">" + row[column] + "</option>";
            }

            html += "</select>";
        }
    }
}
