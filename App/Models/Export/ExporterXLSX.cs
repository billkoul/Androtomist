using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using Androtomist.Models.Global;
using System.Text;

public class ExporterXlsx : ExporterAbstract
{
	private const string DecFormat = "#,##0";
	private const string IntFormat = "#,##0";
	private const string StrFormat = "@";
	private const string DateFormat = "m/d/yyyy h:mm:ss";
	private const string PrcFormat = "#,##0.0%";
	private const string FileFond = "Calibri";

	private OfficeOpenXml.Style.XmlAccess.ExcelNamedStyleXml _styleHeader, _styleHyperLink, _styleDataEven, _styleDataOdd;
	private ExcelPackage _excelPackage;
	private ExcelWorkbook _excelBook;
	private readonly int _startRowInd;
	private readonly int _startColInd;
	public bool NoHeader = false;

	public ExporterXlsx(string exportPath) : base(exportPath)
	{
		_excelPackage = new ExcelPackage(new FileInfo(exportPath));
		_excelBook = _excelPackage.Workbook;
		AddStyles(ref _excelBook);

		_startRowInd = 1;
		_startColInd = 1;
	}

	public override void AddSheet()
	{
		ExcelWorksheet excelSheet = CreateSheet(DataTableData.TableName);
		var colList = DataTableData.Columns.Cast<DataColumn>().Where(x => !x.ColumnName.StartsWith("COLOR_")).ToList();
		var colListAll = DataTableData.Columns.Cast<DataColumn>().ToList();

		if (!NoHeader)
		{
			// Header Values
			foreach (DataColumn col in colList)
				excelSheet.Cells[_startRowInd, _startColInd + colList.IndexOf(col)].Value = col.Caption;

			excelSheet.Cells[_startRowInd, _startColInd, _startRowInd, _startColInd + colList.Count - 1].StyleName = _styleHeader.Name;

		}

		foreach (DataRow row in DataTableData.Rows)
		{
			var rowInd = _startRowInd + (!NoHeader ? 1 : 0) + DataTableData.Rows.IndexOf(row);
			foreach (DataColumn col in colListAll)
			{
				var colInd = _startColInd + colList.IndexOf(col);

				if (col.ColumnName.StartsWith("COLOR_"))
				{
					colInd = _startColInd + colList.IndexOf(DataTableData.Columns[col.ColumnName.Substring(6)]);
					excelSheet.Cells[rowInd, colInd, rowInd, colInd].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					excelSheet.Cells[rowInd, colInd, rowInd, colInd].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(row[col].ToString()));
				}
				else
				{
					excelSheet.Cells[rowInd, colInd].Value = col.DataType == typeof(short) ? row[col] : row[col];
				}
			}
		}
		excelSheet.Cells[excelSheet.Dimension.Address].AutoFitColumns();
	}

	public override void Export()
	{
		if (DataTableInfo != null) AddInfoSheet();

		if (_excelPackage.Workbook.Worksheets.Count == 0)
			AddSheet();

		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

		Stream stream = File.Create(ExportPath);
		_excelPackage.SaveAs(stream);
		stream.Close();
	}

	private void AddInfoSheet()
	{
		int rowInd, colInd;
		string formatStr;

		ExcelWorksheet excelSheet = CreateSheet(DataTableInfo.TableName);

		foreach (DataColumn col in DataTableInfo.Columns)
		{
			colInd = _startColInd;
			rowInd = _startRowInd + DataTableInfo.Columns.IndexOf(col);

			formatStr = StrFormat;
			excelSheet.Cells[rowInd, colInd].Style.Numberformat.Format = formatStr;
			excelSheet.Cells[rowInd, colInd].Value = col.ColumnName;
		}
		excelSheet.Cells[_startRowInd, _startColInd, _startRowInd + DataTableInfo.Columns.Count - 1, _startColInd].StyleName = _styleHeader.Name;

		foreach (DataColumn col in DataTableInfo.Columns)
		{
			colInd = _startColInd + 1;
			rowInd = _startRowInd + DataTableInfo.Columns.IndexOf(col);

			formatStr = GetCellFormat(col.DataType);
			excelSheet.Cells[rowInd, colInd].Style.Numberformat.Format = formatStr;
			excelSheet.Cells[rowInd, colInd].Value = DataTableInfo.Rows[0][col];
		}

		excelSheet.Cells[excelSheet.Dimension.Address].AutoFitColumns();
	}


	private string GetCellFormat(Type dataType)
	{
		string formatStr;

		switch (Type.GetTypeCode(dataType))
		{
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
				{
					formatStr = IntFormat;
					break;
				}

			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				{
					formatStr = DecFormat; // format_str = prc_format
					break;
				}

			default:
				{
					formatStr = StrFormat;
					break;
				}
		}

		return formatStr;
	}

	private void AddStyles(ref ExcelWorkbook excelBook)
	{
		ExportColors exportColors = new ExportColors();

		_styleHyperLink = excelBook.Styles.CreateNamedStyle("HyperLink");
		_styleHyperLink.Style.Font.UnderLine = true;
		_styleHyperLink.Style.Font.Color.SetColor(Color.Blue);

		_styleHeader = excelBook.Styles.CreateNamedStyle("Header");
		{
			var withBlock = _styleHeader.Style;
			withBlock.Font.Bold = true;
			withBlock.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
			withBlock.Fill.BackgroundColor.SetColor(exportColors.HeaderFill);
			withBlock.Font.Color.SetColor(exportColors.HeaderFont);
			withBlock.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
			withBlock.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
			withBlock.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
			withBlock.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
			withBlock.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
			withBlock.Border.Top.Color.SetColor(exportColors.HeaderBorder);
			withBlock.Border.Bottom.Color.SetColor(exportColors.HeaderBorder);
			withBlock.Border.Left.Color.SetColor(exportColors.HeaderBorder);
			withBlock.Border.Right.Color.SetColor(exportColors.HeaderBorder);
		}

		_styleDataEven = excelBook.Styles.CreateNamedStyle("Even");

		// Using range = excel_sheet.Cells(2, 1, data_table.Rows.Count + 1, data_table.Columns.Count)
		{
			var withBlock1 = _styleDataEven.Style;
			withBlock1.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
			withBlock1.Fill.BackgroundColor.SetColor(exportColors.DataEvenFill);
			withBlock1.Font.Color.SetColor(exportColors.DataFont);

			withBlock1.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
			withBlock1.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
			withBlock1.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
			withBlock1.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

			withBlock1.Border.Top.Color.SetColor(exportColors.DataBorder);
			withBlock1.Border.Bottom.Color.SetColor(exportColors.DataBorder);
			withBlock1.Border.Left.Color.SetColor(exportColors.DataBorder);
			withBlock1.Border.Right.Color.SetColor(exportColors.DataBorder);
		}

		_styleDataOdd = excelBook.Styles.CreateNamedStyle("Odd");
		{
			var withBlock2 = _styleDataOdd.Style;
			withBlock2.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
			withBlock2.Fill.BackgroundColor.SetColor(exportColors.DataOddFill);
			withBlock2.Font.Color.SetColor(exportColors.DataFont);

			withBlock2.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
			withBlock2.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
			withBlock2.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
			withBlock2.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

			withBlock2.Border.Top.Color.SetColor(exportColors.DataBorder);
			withBlock2.Border.Bottom.Color.SetColor(exportColors.DataBorder);
			withBlock2.Border.Left.Color.SetColor(exportColors.DataBorder);
			withBlock2.Border.Right.Color.SetColor(exportColors.DataBorder);
		}
	}

	private void AddBookInfo()
	{
		ProjectProperties projectProperties = new ProjectProperties();

		_excelPackage.Workbook.Properties.Title = projectProperties.Name + " " + projectProperties.Verions + " automated export";
		_excelPackage.Workbook.Properties.Subject = projectProperties.Name + " " + projectProperties.Verions + " automated export";
		_excelPackage.Workbook.Properties.Author = projectProperties.Author;
		_excelPackage.Workbook.Properties.Comments = "This file is an automated export by " + projectProperties.Name + " software";
		_excelPackage.Workbook.Properties.Company = projectProperties.Company;
		_excelPackage.Workbook.Properties.HyperlinkBase = new System.Uri(projectProperties.URL);
	}

	protected ExcelWorksheet CreateSheet(string sheetName)
	{
		List<string> forbidenStrings = new List<string>() { "/", @"\", "?", "*", "]", "[", ":", "'" };
		ExcelWorksheet excelSheet;
		string sheetNameTemp;

		sheetName = new string(sheetName.Select(x => x).Where(x => !forbidenStrings.Contains(x.ToString())).Take(40).ToArray());

		int count = 1;
		excelSheet = _excelBook.Worksheets[sheetName];
		while (excelSheet != null)
		{
			count += 1;
			sheetNameTemp = sheetName + " (" + count + ")";
			excelSheet = _excelBook.Worksheets[sheetNameTemp];
			if (excelSheet == null) sheetName = sheetNameTemp;
		}
		excelSheet = _excelBook.Worksheets.Add(sheetName);
		excelSheet.View.ZoomScale = 85;
		excelSheet.View.PageLayoutView = false;

		return excelSheet;
	}

	public override void CloseDispose()
	{
		base.CloseDispose();

		if (_excelPackage == null) return;
		_excelPackage.Save();

		_excelBook.Dispose();
		_excelBook = null;

		_excelPackage.Dispose();
		_excelPackage = null;
	}


}
