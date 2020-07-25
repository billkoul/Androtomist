using System;
using System.Data;

using System.Linq;

public class ExporterCSV : ExporterAbstract
{
	private System.IO.StreamWriter os;
	private readonly string _delimiter;
	public bool NoHeader = false;

	public ExporterCSV(string exportPath, string delimiter = ",") : base(exportPath)
	{
		os = new System.IO.StreamWriter(exportPath);
		_delimiter = delimiter;
	}

	public override void AddSheet() { }

	public override void Export()
	{
		var colList = DataTableData.Columns.Cast<DataColumn>().ToList();

		// Header Values 
		if (!NoHeader)
		{
			foreach (DataColumn col in colList)
			{
				if (col.Caption == "Number")
					os.Write(col.Caption);
				else
					os.Write("\"" + col.Caption.Replace("\"", "\"\"") + "\"");
				if (colList.Last() != col)
					os.Write(_delimiter);
			}
			os.WriteLine();
		}

		// Data Values 
		foreach (DataRow row in DataTableData.Rows)
		{
			foreach (DataColumn col in colList)
			{
				if (row[col] != DBNull.Value)
				{
					//os.Write("\"" + System.Convert.ToString(BASE.get_ID_value(row[col], col.ColumnName)).Replace("\"", "\"\"") + "\"");

					if (row[col].ToString().Contains(","))
						os.Write("\"" + row[col] + "\"");
					else
						os.Write(row[col]);
				}
				if (colList.Last() != col)
					os.Write(_delimiter);
			}
			os.WriteLine();
		}
	}

	public override void CloseDispose()
	{
		base.CloseDispose();

		if (os == null) return;
		os.Dispose();
		os.Close();
	}
}
