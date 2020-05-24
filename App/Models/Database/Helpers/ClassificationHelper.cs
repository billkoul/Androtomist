using System;
using System.Data;
using Androtomist.Models.Files;
using System.Linq;
using Androtomist.Models.Database.Entities;
using Remotion.Linq.Clauses;

namespace Androtomist.Models.Database.Helpers
{
    public class ClassificationHelper : AbstractHelper
    {
        public string CreateVectors()
        {
            UploadPathHelper _uploadPathHelper = new UploadPathHelper();
            string fileName = "Export_vectors.xlsx";
            string _filePath = System.IO.Path.Combine(_uploadPathHelper.GetUploadPath(true, System.IO.Path.GetRandomFileName()), fileName);
            ExporterXlsx exporterXLSX = new ExporterXlsx(_filePath);

            DataTable dataTableData = new DataTable("DATA") { TableName = "INFO" };
            dataTableData.Columns.Cast<DataColumn>().ToList().ForEach(x => x.ReadOnly = false);
            dataTableData.Columns.Cast<DataColumn>().ToList().ForEach(x => x.MaxLength = 999999999);

            //Info sheet
            DataColumn col = dataTableData.Columns.Add();
            dataTableData.Columns[0].ColumnName = "Info";
            dataTableData.Columns.Add();
            dataTableData.Columns[1].ColumnName = "Details";

            DataRow row1 = dataTableData.NewRow();
            row1[0] = "Export";
            row1[1] = "Androtomist";
            dataTableData.Rows.Add(row1);

            DataRow row2 = dataTableData.NewRow();
            row2[0] = "Date";
            row2[1] = DateTime.Now.ToString();
            dataTableData.Rows.Add(row2);

            DataRow row3 = dataTableData.NewRow();
            row3[0] = "Vectors";
            dataTableData.Rows.Add(row3);

            dataTableData.AcceptChanges();
            exporterXLSX.DataTableData = dataTableData;
            exporterXLSX.AddSheet();

            //Vectors permission vectors
            DataTable dt, vectorsDt;

            dt = databaseConnector.SelectSQL(@"select distinct 'count(permission) FILTER (WHERE permission=''' || permission || ''') AS ' || REPLACE(permission, '.', '') || ',' from r_permissions where permission like '%.permission.%' and file_id in (select file_id from a_file where file_label IN (select goodware_file_label from a_info) or file_label IN (select malware_file_label from a_info))", "");

            string sql = @"
            select 
                file_id, ";

            foreach (DataRow row in dt.Rows)
                sql += row[0];

            sql += @"
                CASE 
		            WHEN file_id in (select file_id from a_file where file_label in (select malware_file_label from a_info)) THEN 1
		            ELSE 0
	            END AS is_malware
	            FROM r_permissions
                where (file_id in (select file_id from a_file where file_label in (select malware_file_label from a_info)
								 or file_label in (select goodware_file_label from a_info)))
            GROUP BY file_id
            ORDER BY file_id";

            vectorsDt = databaseConnector.SelectSQL(sql, "");

            //TODO export vectors from all features

            exporterXLSX.DataTableData = vectorsDt;
            exporterXLSX.DataTableData.TableName = "Permissions";
            exporterXLSX.AddSheet();

            exporterXLSX.Export();
            exporterXLSX.CloseDispose();

            return _uploadPathHelper.GetDownloadUrl(_filePath);
        }
    }
}