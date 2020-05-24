using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace Androtomist.Models.Database.Datatables
{
    public class UploadsDatatable : AbstractDatatable
    {
        public UploadsDatatable(DataTableRequest dataTableRequest, long UNIQUE_ID = -1) : base(dataTableRequest, UNIQUE_ID)
        {
            TABLE_TYPE = TABLE_TYPE.SUBMISSION;

            SQL = @"
                SELECT 
                    A_FILE.FILE_ID,
                    A_FILE.FILE_NAME,
                    A_FILE.FILE_LUD
                FROM A_FILE
                WHERE IS_SAMPLE IS NULL --THESE ARE SAMPLES
                ORDER BY A_FILE.FILE_LUD DESC
            ";
        }

        public override DataTableHtml GetDataTableHtml()
        {
            DataTableHtml dataTableHtml = new DataTableHtml
            {
                TABLE_TYPE = TABLE_TYPE,
                UNIQUE_ID = UNIQUE_ID,
                TITLE = "Files",
                ENTITY_NAME = "Submission",
                DEFAULT_ORDER_COL = 2,
                CAN_REMOVE = false,
                URL_ADD = "/files/files/add/",               
                COLUMNS = new DataTableHtml.DataColumn[]
                {
                    new DataTableHtml.DataColumn("ACTIONS", "Actions",  false, false),
                    new DataTableHtml.DataColumn("FILE_NAME", "File Name"),
                    new DataTableHtml.DataColumn("FILE_LUD", "Upload Date")
                }
            };

            return dataTableHtml;
        }

        public override DataTableResult GetDataTableResult(string offset = "")
        {
            DataTable dataTable = GetData(offset);
            DataTableHtml dataTableHtml = GetDataTableHtml();

            DataTableResult dataTableResult = new DataTableResult()
            {
                iTotalRecords = GetRowsLength(false),
                iTotalDisplayRecords = GetRowsLength(true),
                sColumns = string.Empty,
                sEcho = dataTableRequest.sEcho,
                aaData = dataTable.Rows.Cast<DataRow>().Select(x =>
                {
                    dynamic d = new ExpandoObject() as IDictionary<string, object>;
                    var dictionary = (IDictionary<string, object>)d;
                    List<string> exclude_cols = new List<string>() {"ACTIONS" };

                    dataTableHtml.COLUMNS.Where(c => !exclude_cols.Contains(c.NAME)).ToList().ForEach(c => dictionary.Add(c.NAME, Convert.ToString(x[c.NAME])));

                    dictionary.Add("ACTIONS", @" 
                        <span class=""dropdown""> 
                            <a href=""#"" class=""btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill"" data-toggle=""dropdown"" aria-expanded=""true""> <i class=""fa fa-ellipsis-h""></i></a>
                            <div class=""dropdown-menu dropdown-menu-right"">
                                <a class=""dropdown-item"" href=""/results/results/file/" + x["FILE_ID"] + @"""><i class=""fa fa-eye""></i> View Results </a> 
                                <a class=""dropdown-item"" href=""/analysis/analysis/file/" + x["FILE_ID"] + @"""><i class=""fa fa-cogs""></i> Analyze </a> 
                                <!--<div class=""dropdown-divider""></div>-->
                                " + (dataTableHtml.CAN_CLONE ? @"<a class=""dropdown-item clone-row"" href=""#"" data-row_id=""" + x["FILE_ID"] + @"""><i class=""fa fa-clone""></i> Clone</a> " : "") + @"
                                " + (dataTableHtml.CAN_REMOVE ? @"<a class=""dropdown-item delete-row"" href=""#"" data-row_id=""" + x["FILE_ID"] + @"""><i class=""fa fa-remove""></i> Remove</a> " : "") + @"
                            </div>
                        </span>
                    ");

                    return d;
                }).ToList()
            };

            return dataTableResult;
        }


    }
}
