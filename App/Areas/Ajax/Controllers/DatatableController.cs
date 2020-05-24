using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Androtomist.Models.Database.Datatables;
using Androtomist.Models.Database.Helpers;
using Androtomist.Models.Files;
using Androtomist.Models.Forms;

namespace Androtomist.Areas.Ajax.Controllers
{
    [Area("Ajax")]
    public class DatatableController : Controller
    {
        public IActionResult Index()
        {
            return NoContent();
        }

        public IActionResult GetTable([FromForm] DataTableRequest dataTableRequest, string uniqueId, string extraFilter)
        {
            AbstractDatatable abstractDatatable;
            long.TryParse(uniqueId, out long UNIQUE_ID);

            switch (dataTableRequest.tabletype)
            {
                case TABLE_TYPE.USER: abstractDatatable = new UsersDatatable(dataTableRequest, UNIQUE_ID); break;
                case TABLE_TYPE.SUBMISSION: abstractDatatable = new UploadsDatatable(dataTableRequest, UNIQUE_ID); break;
				case TABLE_TYPE.PROCESS: abstractDatatable = new ProcessDatatable(dataTableRequest, UNIQUE_ID); break;
                case TABLE_TYPE.RESULTS: abstractDatatable = new ResultsDatatable(dataTableRequest, UNIQUE_ID); break;

                default: throw new Exception("Wrong datatable type.");
            }

            return Json(abstractDatatable.GetDataTableResult());
        }

        public IActionResult ExportTable([FromForm] DataTableRequest dataTableRequest, string uniqueId, string extraFilter, string export_type, string file_name, string order)
        {
            ExporterAbastract exporterAbastract = null;
            DataTable dataTable;
            AbstractDatatable abstractDatatable;
            string file_path = "";
            long.TryParse(uniqueId, out long UNIQUE_ID);

            string[] orders = order.Split(',');
            DtOrder[] dt = new DtOrder[1];
            dt[0] = new DtOrder
            {
                column = int.Parse(orders[0]),
                dir = orders[1]
            };
            dataTableRequest.order = dt;

            switch (dataTableRequest.tabletype)
            {
                case TABLE_TYPE.USER: abstractDatatable = new UsersDatatable(dataTableRequest, UNIQUE_ID); break;
                case TABLE_TYPE.SUBMISSION: abstractDatatable = new UploadsDatatable(dataTableRequest, UNIQUE_ID); break;
                case TABLE_TYPE.PROCESS: abstractDatatable = new ProcessDatatable(dataTableRequest, UNIQUE_ID); break;
                case TABLE_TYPE.RESULTS: abstractDatatable = new ResultsDatatable(dataTableRequest, UNIQUE_ID); break;

                default: throw new Exception("Wrong datatable type.");
            }

            FormResponse formResponse = new FormResponse();
            UploadPathHelper uploadPathHelper = new UploadPathHelper();


            DataTableHtml dataTableHtml = abstractDatatable.GetDataTableHtml();
            dataTable = abstractDatatable.GetData(" OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY ");
            dataTable.Clear();

            foreach (var colIndex in dataTableHtml.COLUMNS.Select((x, i) => new { index = i, col = x }))
            {
                if (colIndex.col.NAME.Equals("SELECT") || colIndex.col.NAME.Equals("ACTIONS"))
                    continue;

                if (!dataTable.Columns.Contains(colIndex.col.NAME))
                    dataTable.Columns.Add(colIndex.col.NAME, typeof(string));

                dataTable.Columns[colIndex.col.NAME].Caption = colIndex.col.TITLE;
                dataTable.Columns[colIndex.col.NAME].SetOrdinal(colIndex.index);
            }

            dataTable.Columns.Cast<DataColumn>().ToList()
                 .Where(x => !dataTableHtml.COLUMNS.Select(y => y.NAME).ToList().Contains(x.ColumnName.ToUpper()))
                 .ToList().ForEach(x => x.Table.Columns.Remove(x));

            dataTable.BeginLoadData();

            foreach (IDictionary<string, object> r in abstractDatatable.GetDataTableResult(" OFFSET 0 ROWS ").aaData)
            {
                DataRow row = dataTable.NewRow();

                foreach (DataColumn col in dataTable.Columns)
                {
                    if (col.ColumnName.Equals("EXCLUDE"))
                        if (r[col.ColumnName].ToString().Contains("checked")) row[0] = 1; else row[0] = 0;
                    else
                        row[col] = r[col.ColumnName.ToUpper()];
                }

                dataTable.Rows.Add(row);
            }

            dataTable.EndLoadData();

            dataTable.AcceptChanges();


            if (export_type == "excel")
            {
                file_path = System.IO.Path.Combine(uploadPathHelper.GetUploadPath(true, System.IO.Path.GetRandomFileName()), file_name + ".xlsx");
                exporterAbastract = new ExporterXlsx(file_path)
                {
                    DataTableData = dataTable
                };
            }
            else if (export_type == "csv")
            {
                file_path = System.IO.Path.Combine(uploadPathHelper.GetUploadPath(true, System.IO.Path.GetRandomFileName()), file_name + ".csv");
                exporterAbastract = new ExporterCSV(file_path)
                {
                    DataTableData = dataTable
                };
            }

            exporterAbastract.Export();

            exporterAbastract.CloseDispose();

            formResponse.msg = uploadPathHelper.GetDownloadUrl(file_path);

            return Json(formResponse);
        }

        public IActionResult GetColumns([FromForm] DropDownRequest dropDownRequest, string uniqueId, string extraFilter)
        {
            AbstractDatatable abstractDatatable;
            long.TryParse(uniqueId, out long UNIQUE_ID);

            switch (dropDownRequest.tabletype)
            {
                case TABLE_TYPE.USER: abstractDatatable = new UsersDatatable(new DataTableRequest(), UNIQUE_ID); break;
                case TABLE_TYPE.SUBMISSION: abstractDatatable = new UploadsDatatable(new DataTableRequest(), UNIQUE_ID); break;
                case TABLE_TYPE.PROCESS: abstractDatatable = new ProcessDatatable(new DataTableRequest(), UNIQUE_ID); break;
                case TABLE_TYPE.RESULTS: abstractDatatable = new ResultsDatatable(new DataTableRequest(), UNIQUE_ID); break;

                default: throw new Exception("Wrong datatable type.");
            }

            return Json(abstractDatatable.GetColumnDropDown(dropDownRequest));
        }

        public IActionResult RemoveRows(string id, string uniqueId, string rowIDs)
        {
            FormResponse formResponse = new FormResponse();

            try
            {
                List<long> rowIDsArray = JsonConvert.DeserializeObject<List<string>>(rowIDs).Select(x => Convert.ToInt64(x)).ToList();

                if (rowIDsArray.Count == 0) throw new FormDataException("No rows selected");

                if (Enum.TryParse(id, out TABLE_TYPE tabletype))
                {
                    switch (tabletype)
                    {
                        case TABLE_TYPE.USER:
                            UserHelper userHelper = new UserHelper();
                            userHelper.RemoveUsers(rowIDsArray);
                            break;
						case TABLE_TYPE.SUBMISSION:
							SubmissionHelper submissionHelper = new SubmissionHelper();
							submissionHelper.RemoveSubmission(rowIDsArray);
							break;
                        case TABLE_TYPE.PROCESS:
							ProcessHelper processHelper = new ProcessHelper();
                            processHelper.RemoveProcess(rowIDsArray);
							break;
                        case TABLE_TYPE.RESULTS:
                            ResultsHelper resultsHelper = new ResultsHelper();
                            resultsHelper.RemoveResults(rowIDsArray);
                            break;

                        default: throw new Exception("Wrong datatable type.");
                    }
                }
                formResponse.msg = "Rows removed succesfully.";
            }
            catch (FormDataException ex)
            {
                formResponse.result = 0;
                formResponse.msg = ex.Message;
            }
            catch (Exception ex)
            {
                formResponse.result = 0;
                formResponse.msg = ex.Message;
            }

            return Json(formResponse);
        }

        public IActionResult CloneRows(string id, string uniqueId, string rowIDs)
        {
            FormResponse formResponse = new FormResponse();

            try
            {
                List<long> rowIDsArray = JsonConvert.DeserializeObject<List<string>>(rowIDs).Select(x => Convert.ToInt64(x)).ToList();

                if (rowIDsArray.Count == 0) throw new FormDataException("No rows selected");

                if (Enum.TryParse(id, out TABLE_TYPE tabletype))
                {
                    switch (tabletype)
                    {
                        default: throw new Exception("Wrong datatable type.");
                    }
                }
                formResponse.msg = "Rows cloned succesfully.";
            }
            catch (FormDataException ex)
            {
                formResponse.result = 0;
                formResponse.msg = ex.Message;
            }
            catch (Exception ex)
            {
                formResponse.result = 0;
                formResponse.msg = ex.Message;
            }

            return Json(formResponse);
        }

        public IActionResult ExcludeRows(string id, string uniqueId, string rowIDsInclude, string rowIDsExclude)
        {
            FormResponse formResponse = new FormResponse();

            try
            {
                if (!Enum.TryParse(id, out TABLE_TYPE TABLE_TYPE) || !Enum.IsDefined(typeof(TABLE_TYPE), TABLE_TYPE)) throw new Exception("Wrong table type");

                long.TryParse(uniqueId, out long UNIQUE_ID);
                string sesKey = "_exclude_" + uniqueId;

                List<string> rowIDsArrayInclude = JsonConvert.DeserializeObject<List<string>>(rowIDsInclude).ToList();
                List<string> rowIDsArrayExclude = JsonConvert.DeserializeObject<List<string>>(rowIDsExclude).ToList();
                if (rowIDsArrayExclude.Count + rowIDsArrayInclude.Count == 0) throw new FormDataException("No rows selected");

                // Get all non selected by this page
                DataTableExclude dataTableInclude = new DataTableExclude()
                {
                    UNIQUE_ID = UNIQUE_ID,
                    KEYS = rowIDsArrayInclude.ToList(),
                    TABLE_TYPE = TABLE_TYPE
                };

                // Get session and remove non selected for this page
                DataTableExclude dataTableExcludeSession = null;
                System.Web.HttpContext.Current.Session.TryGetValue(sesKey, out byte[] sessionBytes);
                if (sessionBytes != null && sessionBytes.Length > 0)
                {
                    var sessionString = System.Text.Encoding.Default.GetString(sessionBytes);
                    dataTableExcludeSession = JsonConvert.DeserializeObject<DataTableExclude>(sessionString);
                    if (!dataTableExcludeSession.UNIQUE_ID.Equals(dataTableInclude.UNIQUE_ID)) throw new FormDataException("Session elements are not equal.");
                    dataTableExcludeSession.KEYS = dataTableExcludeSession.KEYS.Except(dataTableInclude.KEYS).ToList();
                }

                // Concat session and current selection
                DataTableExclude dataTableExclude = new DataTableExclude()
                {
                    UNIQUE_ID = UNIQUE_ID,
                    KEYS = rowIDsArrayExclude.ToList(),
                    TABLE_TYPE = TABLE_TYPE
                };

                if (dataTableExcludeSession != null) dataTableExclude.KEYS = dataTableExclude.KEYS.Union(dataTableExcludeSession.KEYS).ToList();

                HttpContext.Session.Set(sesKey, System.Text.Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(dataTableExclude)));

                formResponse.msg = "Rows excluded succesfully.";
            }
            catch (FormDataException ex)
            {
                formResponse.result = 0;
                formResponse.msg = ex.Message;
            }
            catch (Exception ex)
            {
                formResponse.result = 0;
                formResponse.msg = ex.Message;
            }

            return Json(formResponse);
        }

    }
}