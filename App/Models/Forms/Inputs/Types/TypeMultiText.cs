using System;
using System.Data;
using Androtomist.Models.Forms;

namespace Androtomist.Models.Database.Inputs
{
    public class TypeMultiText : TypeAbstract
    {
        public TypeMultiText(string tableName = "", string column = "", string name = "", string extraFilter = "", string fitlerWith = "") : base()
        {
            this.name = name;
			this.tableName = tableName;
			this.current = current;
			this.current = current;

			Populate("SELECT * FROM " + tableName + (!string.IsNullOrEmpty(extraFilter) ? " WHERE "+column+" = "+extraFilter : " WHERE 1=2"), tableName);
        }

        public override void Populate(string SQL, string tableName)
        {
			DataTable dt = databaseConnector.SelectSQL(SQL, tableName);

			html += "<div class='col-md-4' id='copy1' target='" + name + "' style='display:none;'>" +
					"<div class='form-group m-form__group' style='border:1px solid #ccc; padding: 20px;'>";
			foreach (DataColumn col in dt.Columns)
			{
				if (col.Caption.Contains("id"))
					html += "<input type='hidden' class='form-control m-input' id='" + name + "[" + col.Caption + "]' name='" + name + "[" + col.Caption + "]' value='' />";
				else
				{
					html += "<label for='" + col.Caption + "'>" + col.Caption.Replace('_', ' ') + "</label><span class='m-form__help'></span>";
					if (col.Caption.Contains("date"))
					{
						DateTime dateT = DateTime.Now;
						html += "<input type='date' class='form-control m-input' id='" + name + "[" + col.Caption + "]' name='" + name + "[" + col.Caption + "]' value='" + dateT.ToString("yyyy-MM-dd") + "' />";
					}
					else
						html += "<input type='text' class='form-control m-input' id='" + name + "[" + col.Caption + "]' name='" + name + "[" + col.Caption + "]' value='' />";
				}
			}
			html += "</div>" +
					"</div>";

			if (dt != null && dt.Rows.Count > 0)
			{
				int i = 2;
				foreach (DataRow row in dt.Rows)
				{
					html += "<div class='col-md-4' id='copy" + i + "' target='" + name + "'>" +
						"<div class='form-group m-form__group' style='border:1px solid #ccc; padding: 20px;'>";
					foreach (DataColumn col in row.Table.Columns)
					{
						if (col.Caption.Contains("id") && col.Ordinal == 0)
							html += "<input type='hidden' class='form-control m-input has_id' id='" + name + "[" + col.Caption + "]' name='" + col.Caption + "[" + i + "]' value='" + row[col.Caption] + "'/>";
						else if (col.Caption.Contains("id"))
							html += "<input type='hidden' class='form-control m-input' id='" + name + "[" + col.Caption + "]' name='" + col.Caption + "[" + i + "]' value='" + row[col.Caption] + "'/>";
						else
						{
							html += "<label for='" + col.Caption + "'>" + col.Caption.Replace('_', ' ') + "</label><span class='m-form__help'></span>";
							if (col.Caption.Contains("date"))
							{
								DateTime dateT = (DateTime.TryParse(row[col.Caption].ToString(), out dateT) ? dateT : DateTime.Now);
								html += "<input type='date' class='form-control m-input' id='" + name + "[" + col.Caption + "]' name='" + col.Caption + "[" + i + "]' value='" + dateT.ToString("yyyy-MM-dd") + "' />";
							}
							else
								html += "<input type='text' class='form-control m-input' id='" + name + "[" + col.Caption + "]' name='" + col.Caption + "[" + i + "]' value='" + row[col.Caption] + "' />";
						}
					}
					html += "</div>" +
						"</div>";
					i++;
				}
			}
		}
    }
}
