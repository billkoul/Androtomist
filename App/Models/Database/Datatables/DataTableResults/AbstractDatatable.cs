using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Androtomist.Models.Forms;
using Androtomist.Models.Global;

namespace Androtomist.Models.Database.Datatables
{
	public abstract class AbstractDatatable : DBClass
	{
		public readonly long UNIQUE_ID;

		protected TABLE_TYPE TABLE_TYPE { get; set; }
		protected string SQL { get; set; }
		protected readonly DataTableRequest dataTableRequest;
		protected string ExtraFilter { get; set; }

		public AbstractDatatable(DataTableRequest dataTableRequest, long UNIQUE_ID)
		{
			this.dataTableRequest = dataTableRequest;

			this.UNIQUE_ID = UNIQUE_ID;
			if (UNIQUE_ID <= 0)
			{
				MathFuncs mathFuncs = new MathFuncs();
				this.UNIQUE_ID = mathFuncs.GetRandomLong(0, long.MaxValue);
			}
		}



		public abstract DataTableHtml GetDataTableHtml();

		public abstract DataTableResult GetDataTableResult(string offset = "");


		public int GetRowsLength(bool addFilter)
		{
			DataTable dataTable;
			int rowsLength = 0;

			dataTable = databaseConnector.SelectSQL("SELECT COUNT(*) AS COUNTA FROM (" + SQL + ") T1" + (addFilter ? SQL_WHERE(dataTableRequest.GetFilteredColumns()) : ""), "COUNT");

			if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["COUNTA"] != DBNull.Value)
				rowsLength = Convert.ToInt32(dataTable.Rows[0]["COUNTA"]);

			return rowsLength;
		}


		public DropDown GetColumnDropDown(DropDownRequest dropDownRequest)
		{
			DropDown dropDown = new DropDown();
			DataTable dataTable;

			if (dropDownRequest.column_name.Equals("EXCLUDE"))
			{
				dropDown.total_count = 2;
				dropDown.page_length = 2;
				dropDown.items = new DropDownItem[] {
					new DropDownItem("1", "YES" + ""),
					new DropDownItem("0", "NO" + "")
				};

				return dropDown;
			}

			dataTable = databaseConnector.SelectSQL("SELECT " + dropDownRequest.column_name + ", COUNT(" + dropDownRequest.column_name + ") AS COUNTA FROM (" + SQL + ") T1 " + SQL_WHERE(dropDownRequest.GetFilteredColumns(), dropDownRequest.column_name, dropDownRequest.q) + " GROUP BY " + dropDownRequest.column_name + " ORDER BY " + dropDownRequest.column_name, "DROP_DOWN");

			dropDown.total_count = 0;
			dropDown.page_length = 20;
			var obj = dataTable.Rows.Cast<DataRow>()
				.Select(x => new { text = x[dropDownRequest.column_name].ToString().Replace("\0", ""), count = Convert.ToInt32(x["COUNTA"]) })
				.Select(x => new { text = string.IsNullOrWhiteSpace(x.text) ? "[NULL]" : x.text, x.count })
				.OrderByDescending(x =>
				{
					if (dropDownRequest.column_name.EndsWith("_DATE") || dropDownRequest.column_name.EndsWith("_DATETIME") || dropDownRequest.column_name.EndsWith("_TIME"))
					{
						if (DateTime.TryParse(x.text, out DateTime return_date))
							return return_date.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
					}
					return "99999999999999";
				})
				.ThenBy(x =>
				{
					if (!(dropDownRequest.column_name.EndsWith("_DATE") || dropDownRequest.column_name.EndsWith("_DATETIME") || dropDownRequest.column_name.EndsWith("_TIME")))
						return Regex.Replace(x.text, "[0-9]+", match => match.Value.PadLeft(20, '0'));
					return "";
				})
				.GroupBy(x => x.text, StringComparer.InvariantCultureIgnoreCase)
				.Select(x => new { x.First().text, count = x.Sum(y => y.count) });
			dropDown.total_count = obj.Count();

			dropDown.items = obj
				.Skip((dropDownRequest.page - 1) * dropDown.page_length).Take(dropDown.page_length)
				.Select(x => new DropDownItem(x.text, x.text + " (" + x.count + ")"))
				.ToArray();


			return dropDown;
		}

		public DataTable GetData(string default_offset)
		{
			string selectSQL;

			if (string.IsNullOrEmpty(default_offset))
				selectSQL = "SELECT * FROM (" + SQL + ") T1 " + SQL_WHERE(dataTableRequest.GetFilteredColumns()) + SQL_ORDER + SQL_OFFSET;
			else
				selectSQL = "SELECT * FROM (" + SQL + ") T1 " + SQL_WHERE(dataTableRequest.GetFilteredColumns()) + SQL_ORDER + default_offset;

			return databaseConnector.SelectSQL(selectSQL, "DATA");
		}

		public string escape(string str)
		{
			return str.Replace("'", "''").Replace("+", "\\+").Replace("[", "\\[").Replace("]", "\\]").Replace(")", "\\)").Replace("(", "\\(");
		}

		protected virtual string SQL_WHERE(List<DropDownColumns.FilteredColumn> filteredColumns, string dropdown_column = "", string q = "")
		{
			string SQL = string.Empty;

			SQL = string.Join(") AND (", filteredColumns.Where(x => (x.filter_vals != null && x.filter_vals.Count > 0) || (x.column_name.Equals(dropdown_column) && !string.IsNullOrWhiteSpace(q))).Select(x =>
			{
				List<string> SQL_inner = new List<string>();

				if (!x.column_name.Equals(dropdown_column))
				{
					if (x.filter_vals.Count(y => y.id.Equals("[NULL]")) > 0)
						SQL_inner.Add(x.column_name + " IS NULL");
					if (x.filter_vals.Count(y => y.is_tag && !y.id.Equals("[NULL]")) > 0)
						SQL_inner.Add("lower(" + x.column_name + "::TEXT) ~ '" + string.Join("|", x.filter_vals.Where(y => y.is_tag && !y.id.Equals("[NULL]")).Select(y => escape(y.id).ToLower().Trim()).ToArray()) + "'::TEXT");
					if (x.filter_vals.Count(y => !y.is_tag && !y.id.Equals("[NULL]")) > 0)
						SQL_inner.Add("lower(" + x.column_name + "::TEXT) ~ '^" + string.Join("$|^", x.filter_vals.Where(y => !y.is_tag && !y.id.Equals("[NULL]")).Select(y => escape(y.id).ToLower().Trim()).ToArray()) + "$'::TEXT");
				}
				else if (!string.IsNullOrWhiteSpace(q))
				{
					SQL_inner.Add("lower(" + x.column_name + "::TEXT) ~ lower('" + escape(q).ToUpper().Trim() + "'::TEXT)");
				}

				return string.Join(" OR ", SQL_inner.ToArray());
			}).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray());
			if (!string.IsNullOrWhiteSpace(SQL)) SQL = " WHERE " + " (" + SQL + ")";

			return SQL;

		}

		private string SQL_ORDER
		{
			get
			{
				if (dataTableRequest.order == null || dataTableRequest.order.Length == 0)
					return string.Empty; //throw new Exception("No order column.");

				string order = string.Join(",", dataTableRequest.order.Select(x =>
				{
					string orderStr;

					orderStr = "T1." + dataTableRequest.columns[x.column].name;

					// For null datetimes
					/*if (orderStr.IndexOf("TO_DATE") == 0)
                    {
                        orderStr = @"(
                            CASE T1." + dataTableRequest.columns[x.column].name + @"
                                WHEN 'NEVER' THEN TO_DATE('01 JAN 2000', 'DD MON YYYY')
                                ELSE " + orderStr + @"
                            END)
                        ";
                    }*/

					return orderStr + " " + x.dir;
				}).ToArray());
				if (!string.IsNullOrEmpty(order)) order = " ORDER BY " + order;
				return order;
			}
		}

		protected string SQL_OFFSET
		{
			get
			{
				if (dataTableRequest.order == null || dataTableRequest.order.Length == 0) return string.Empty; //throw new Exception("No offset column.");

				return " OFFSET " + dataTableRequest.start + " ROWS FETCH NEXT " + dataTableRequest.length + " ROWS ONLY ";
			}
		}

	}
}


