using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace Androtomist.Models.Database.Datatables
{
    public class ResultsDatatable : AbstractDatatable
    {
        public ResultsDatatable(DataTableRequest dataTableRequest, long UNIQUE_ID = -1) : base(dataTableRequest, UNIQUE_ID)
        {
            TABLE_TYPE = TABLE_TYPE.RESULTS;

            SQL = @"
                SELECT 
                    D_RESULTS.*,
                    C_PROCESS.P_NAME,
                    C_PROCESS.P_DATE,
                    A_PROCESS_TYPE.PTYPE_NAME,
                    A_FILE.FILE_NAME,
                    A_WARNING_LEVEL.LEVEL_NAME
                FROM D_RESULTS
                    INNER JOIN C_PROCESS ON C_PROCESS.P_ID = D_RESULTS.R_P_ID
                    INNER JOIN A_FILE ON A_FILE.FILE_ID = C_PROCESS.P_FILE_ID
                    LEFT JOIN A_PROCESS_TYPE ON A_PROCESS_TYPE.PTYPE_ID = C_PROCESS.P_TYPE_ID
                    LEFT JOIN A_WARNING_LEVEL ON A_WARNING_LEVEL.LEVEL_ID = D_RESULTS.R_WARNING_LEVEL
                ORDER BY D_RESULTS.R_ID DESC
            ";
        }

        public override DataTableHtml GetDataTableHtml()
        {
            DataTableHtml dataTableHtml = new DataTableHtml
            {
                TABLE_TYPE = TABLE_TYPE,
                UNIQUE_ID = UNIQUE_ID,
                TITLE = "Results",
                ENTITY_NAME = "Results",
                DEFAULT_ORDER_COL = 2,
                URL_ADD = "#",               
                COLUMNS = new DataTableHtml.DataColumn[]
                {
                    new DataTableHtml.DataColumn("ACTIONS", "Actions",  false, false),
                    new DataTableHtml.DataColumn("P_NAME", "Analysis"),
                    new DataTableHtml.DataColumn("FILE_NAME", "File"),
                    new DataTableHtml.DataColumn("P_DATE", "Date processed"),
                    new DataTableHtml.DataColumn("P_TYPE", "Type"),
                    new DataTableHtml.DataColumn("R_WARNING", "Warning level")
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
                    List<string> exclude_cols = new List<string>() { "ACTIONS" };

                    dataTableHtml.COLUMNS.Where(c => !exclude_cols.Contains(c.NAME)).ToList().ForEach(c => dictionary.Add(c.NAME, Convert.ToString(x[c.NAME])));

                    dictionary.Add("ACTIONS", @" 
                        <span class=""dropdown""> 
                            <a href=""#"" class=""btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill"" data-toggle=""dropdown"" aria-expanded=""true""> <i class=""fa fa-ellipsis-h""></i></a>
                            <div class=""dropdown-menu dropdown-menu-right"">
                                <a class=""dropdown-item"" href=""/results/results/view/" + x["R_ID"] + @"""><i class=""fa fa-bar-chart""></i> View </a> 
                                <div class=""dropdown-divider""></div>
                                " + (dataTableHtml.CAN_CLONE ? @"<a class=""dropdown-item clone-row"" href=""#"" data-row_id=""" + x["R_ID"] + @"""><i class=""fa fa-clone""></i> Clone</a> " : "") + @"
                                " + (dataTableHtml.CAN_REMOVE ? @"<a class=""dropdown-item delete-row"" href=""#"" data-row_id=""" + x["R_ID"] + @"""><i class=""fa fa-remove""></i> Remove</a> " : "") + @"
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
