using System;
using System.Data;
using Androtomist.Models.Forms;

namespace Androtomist.Models.Database.Inputs
{
    public class TypeMultiSelect : TypeAbstract
    {
        public TypeMultiSelect(string tableName = "", string column = "", string name = "", string extraFilter = "", string fitlerWith = "") : base()
        {
            this.name = name;
			this.tableName = tableName;
			this.current = extraFilter;
			this.column = column;
			this.fitlerWith = fitlerWith;

			Populate("SELECT * FROM " + tableName, tableName);
        }

        public override void Populate(string SQL, string tableName)
        {
			DataTable dt_copy = databaseConnector.SelectSQL(SQL, tableName);
			DataTable dt = databaseConnector.SelectSQL(SQL + (!string.IsNullOrEmpty(fitlerWith) ? fitlerWith : ""), tableName);

			html += "<div class='col-md-4' id='copy1' target='" + name + "' style='display:none;'>" +
					"<div class='form-group m-form__group' style='border:1px solid #ccc; padding: 20px;'>" +
				    "<label for='" + column + "'>" + column.Replace('_', ' ') + "</label><span class='m-form__help'></span>" +
					"<select class='form-control m-input' id='" + name + "[" + column + "]' name='" + name + "[" + column + "]'>";

			foreach (DataRow row in dt_copy.Rows)
			{
				html += "<option value='"+row[0]+"' disabled>"+ row[column] + "</option>";
			}
			
			html += "</select>" +
					"</div>" +
					"</div>";

			int i = 2;
			if (dt != null && dt.Rows.Count > 0)
			{
				foreach (DataRow row in dt.Rows)
				{
					html += "<div class='col-md-4' id='copy" + i + "' target='" + name + "'>" +
					"<div class='form-group m-form__group' style='border:1px solid #ccc; padding: 20px;'>" +
					"<label for='" + column + "'>" + column.Replace('_', ' ') + "</label><span class='m-form__help'></span>" +
					"<select class='form-control m-input' id='" + name + "[" + column + "]' name='" + name + "[" + column + "]'>";

					html += "<option value='" + row[0] + "'>" + row[column] + "</option>";

					html += "</select>" +
							"</div>" +
							"</div>";
					i++;
				}
			}
		}
    }
}
