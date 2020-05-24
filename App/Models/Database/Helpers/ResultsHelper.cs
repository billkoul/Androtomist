using System;
using System.Collections.Generic;
using System.Data;
using Androtomist.Models.Database.Inserters;
using Androtomist.Models.Results;
using Androtomist.Models.Forms;
using Newtonsoft.Json;
using Androtomist.Models.Files;
using System.Linq;
using Androtomist.Models.Database.Entities;

namespace Androtomist.Models.Database.Helpers
{
    public class ResultsHelper : AbstractHelper
    {
		public Entities.Result AddResult(string createJson)
		{
			FilterData filterData = new FilterData(createJson);

			long.TryParse(filterData.id[0], out long ID);


            ResultsInserter resultsInserter = new ResultsInserter
            {
				R_ID = (ID < 0 ? null : ID.ToString()),
                R_NAME = filterData.R_NAME,
			};
			if (ID > 0) resultsInserter.Insert();
			else ID = resultsInserter.Insert(true);

			Entities.Result result = new Entities.Result(ID);
			if (!result.Exists()) throw new Exception("Cannot add results to database");

			return result;
		}

        public void RemoveResults(List<long> IDS)
        {
            int rowsAffected = 0;

            rowsAffected = databaseConnector.DeleteSQL("DELETE FROM D_RESULTS WHERE R_ID IN(" + string.Join(",", IDS) + ")");

            if (rowsAffected == 0)
                throw new Exception("Could not remove any records from database.");

            if (rowsAffected != IDS.Count)
                throw new Exception("Could not remove " + (IDS.Count - rowsAffected) + " records from database.");
        }

        public string Export(long fileId)
        {
            File file = new File(fileId);

            UploadPathHelper _uploadPathHelper = new UploadPathHelper();
            string fileName = "Export_" + fileId + ".xlsx";
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
            row3[0] = "Package Name";
            row3[1] = file.PACKAGE_NAME;
            dataTableData.Rows.Add(row3);

            dataTableData.AcceptChanges();
            exporterXLSX.DataTableData = dataTableData;
            exporterXLSX.AddSheet();

            ResultInfo info = new ResultInfo(fileId);
            info.isExport = true;

            exporterXLSX.DataTableData = info.PERMISSIONS;
            exporterXLSX.DataTableData.TableName = "Permissions";
            exporterXLSX.AddSheet();

            exporterXLSX.DataTableData = info.INTENT;
            exporterXLSX.DataTableData.TableName = "Intents";
            exporterXLSX.AddSheet();

            //uncomment to enable API calls export
            //exporterXLSX.DataTableData = info.API;
            //exporterXLSX.DataTableData.TableName = "API Calls";
            //exporterXLSX.AddSheet();

            exporterXLSX.DataTableData = info.DYNAMIC;
            exporterXLSX.DataTableData.TableName = "Dynamic Analysis";
            exporterXLSX.AddSheet();

            exporterXLSX.Export();
            exporterXLSX.CloseDispose();

            return _uploadPathHelper.GetDownloadUrl(_filePath);
        }

        #region TRAIN
        public void Trainer_InsertPackageName(string FileId = "")
        {
            DataTable sampleDt = databaseConnector.SelectSQL("SELECT FILE_ID FROM A_FILE_TRAIN WHERE FILE_ID < 100", "A_TRAIN_PERM");

            foreach (DataRow row in sampleDt.Rows)
            {
                if (!Int32.TryParse(row["FILE_ID"].ToString(), out int file_id))
                    throw new Exception("Error getting file!");

                DataTable dt = databaseConnector.SelectSQL("SELECT PERMISSION_JSON FROM A_TRAIN_PERM WHERE FILE_ID = " + file_id, "A_TRAIN_PERM");

                if (dt.Rows.Count > 0 && IsValidJson(dt.Rows[0]["PERMISSION_JSON"].ToString()))
                {

                    APK app = JsonConvert.DeserializeObject<APK>(dt.Rows[0]["PERMISSION_JSON"].ToString());

                    List<string> col_names = new List<string>{
                        "package_name",
                    };

                    List<string> col_vals = new List<string> {
                        "'" + app.package + "'",
                    };

                    databaseConnector.UpdateSQL("A_FILE_TRAIN", col_names, col_vals, "FILE_ID="+ file_id);

                }
            }
        }

        public void Trainer_InsertPermissions(string FileId = "")
        {
            DataTable sampleDt = databaseConnector.SelectSQL("select file_id from a_file_train where file_label = 'Playstore2'", "A_TRAIN_PERM");

            foreach (DataRow row in sampleDt.Rows)
            {
                if (!Int32.TryParse(row["FILE_ID"].ToString(), out int file_id))
                    throw new Exception("Error getting file!");

                DataTable dt = databaseConnector.SelectSQL("SELECT PERMISSION_JSON FROM A_TRAIN_PERM WHERE FILE_ID = " + file_id, "A_TRAIN_PERM");

                string permissionText = dt.Rows[0]["PERMISSION_JSON"].ToString().Replace(System.Environment.NewLine, "").Replace(" ", "").Replace("\".","\"").Replace("N]}","\"]}").Replace("t]}", "\"]}");
                permissionText = System.Text.RegularExpressions.Regex.Replace(permissionText, @"\t|\n|\r", "");

                if (dt.Rows.Count > 0 && IsValidJson(permissionText))
                {

                    APK app = JsonConvert.DeserializeObject<APK>(permissionText);

                    databaseConnector.DeleteSQL("DELETE FROM R_PERMISSIONS WHERE FILE_ID = " + file_id);
                    foreach (string permission in app.permissions)
                    {
                        DataTable dt2 = databaseConnector.SelectSQL("SELECT ID FROM A_PERMISSION WHERE VALUE LIKE '" + permission + "'", "A_PERMISSION");
                        int permission_id = (dt2.Rows.Count > 0 ? Convert.ToInt32(dt2.Rows[0]["ID"]) : -1);

                        List<string> col_names = new List<string>{
                            "FILE_ID",
                            "PERMISSION",
                            "PERMISSION_ID"
                        };

                        List<string> col_vals = new List<string> {
                            "'" + file_id + "'",
                            "'" + permission + "'",
                            "'" + permission_id + "'"
                        };

                        databaseConnector.InsertSQL("R_PERMISSIONS", col_names, col_vals);
                    }
                }
                else
                {
                    var a = 1;
                }
            }
        }

        public void Trainer_InsertIntentActions(string FileId = "")
        {
            DataTable sampleDt = databaseConnector.SelectSQL("select file_id from a_file_train where file_label = 'Playstore2'", "A_TRAIN_PERM");

            foreach (DataRow row in sampleDt.Rows)
            {
                if (!Int32.TryParse(row["FILE_ID"].ToString(), out int file_id))
                    throw new Exception("Error getting file!");

                DataTable dt = databaseConnector.SelectSQL("SELECT INTENT_FILTER FROM A_TRAIN_PERM WHERE FILE_ID = " + file_id, "A_TRAIN_PERM");

                var cleanJson = dt.Rows[0]["INTENT_FILTER"].ToString().Replace("$", "").Replace(":\"", "\",\"").Replace(System.Environment.NewLine, "");
                if (dt.Rows.Count > 0 && IsValidJson(cleanJson))
                {

                    APK app = JsonConvert.DeserializeObject<APK>(cleanJson);

                    //databaseConnector.DeleteSQL("DELETE FROM R_INTENT WHERE FILE_ID = " + file_id);
                    foreach (string intent in app.intent)
                    {

                        List<string> col_names = new List<string>{
                            "FILE_ID",
                            "INTENT",
                        };

                        List<string> col_vals = new List<string> {
                            "'" + file_id + "'",
                            "'" + intent + "'",
                        };

                        databaseConnector.InsertSQL("R_INTENT", col_names, col_vals);
                    }
                }
            }
        }

        private bool IsValidJson(string strInput)
        {
            strInput = strInput.Replace("[\",", "[");
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = Newtonsoft.Json.Linq.JToken.Parse(strInput);
                    return true;
                }
                catch
                {
                    //Exception in parsing json
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}