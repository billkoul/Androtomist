using System;
using System.Data;

public abstract class ExporterAbstract
{
	protected string ExportPath;

	public DataTable DataTableData { get; set; }
	public DataTable DataTableInfo { get; set; }

	protected ExporterAbstract(string exportPath)
	{
		this.ExportPath = exportPath;
	}
	public abstract void AddSheet();
	public abstract void Export();

	public virtual void CloseDispose()
	{
		DataTableData?.Dispose();
		DataTableData = null;

		DataTableInfo?.Dispose();
		DataTableInfo = null;

		GC.Collect();
		GC.WaitForPendingFinalizers();
	}
}
