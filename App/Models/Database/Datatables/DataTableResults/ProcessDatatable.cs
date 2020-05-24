using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace Androtomist.Models.Database.Datatables
{
    public class ProcessDatatable : AbstractDatatable
    {
        public ProcessDatatable(DataTableRequest dataTableRequest, long UNIQUE_ID = -1) : base(dataTableRequest, UNIQUE_ID)
        {
            TABLE_TYPE = TABLE_TYPE.PROCESS;

            SQL = @"
                SELECT 
                    C_PROCESS.P_ID,
                    C_PROCESS.P_NAME,
                    A_PROCESS_TYPE.PTYPE_NAME,
                    A_FILE.FILE_NAME AS FILE_NAME,
                    A_FILE.FILE_ID AS FILE_ID
                FROM C_PROCESS
                    LEFT JOIN A_FILE ON A_FILE.FILE_ID = C_PROCESS.P_FILE_ID
                    LEFT JOIN A_PROCESS_TYPE ON A_PROCESS_TYPE.PTYPE_ID = C_PROCESS.P_TYPE_ID
                    
                ORDER BY C_PROCESS.P_ID DESC
            ";
        }

        public override DataTableHtml GetDataTableHtml()
        {
            DataTableHtml dataTableHtml = new DataTableHtml
            {
                TABLE_TYPE = TABLE_TYPE,
                UNIQUE_ID = UNIQUE_ID,
                TITLE = "Processed Files",
                ENTITY_NAME = "Process",
                DEFAULT_ORDER_COL = 2,
                CAN_REMOVE = false,
                URL_ADD = "/analysis/analysis/add/",               
                COLUMNS = new DataTableHtml.DataColumn[]
                {
                    new DataTableHtml.DataColumn("ACTIONS", "Actions",  false, false),
                    new DataTableHtml.DataColumn("P_NAME", "Analysis"),
                    new DataTableHtml.DataColumn("FILE_NAME", "File"),
                    new DataTableHtml.DataColumn("PTYPE_NAME", "Type")
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
                                 " + (dataTableHtml.CAN_REMOVE ? @"<a class=""dropdown-item"" href=""/analysis/analysis/add/" + x["P_ID"] + @"""><i class=""fa fa-edit""></i> Edit </a>" : "") + @"
                                <a class=""dropdown-item"" href=""/results/results/file/" + x["FILE_ID"] + @"""><i class=""fa fa-eye""></i> Results </a> 
                                <!--<div class=""dropdown-divider""></div>-->
                                " + (dataTableHtml.CAN_CLONE ? @"<a class=""dropdown-item clone-row"" href=""#"" data-row_id=""" + x["P_ID"] + @"""><i class=""fa fa-clone""></i> Clone</a> " : "") + @"
                                " + (dataTableHtml.CAN_REMOVE ? @"<a class=""dropdown-item delete-row"" href=""#"" data-row_id=""" + x["P_ID"] + @"""><i class=""fa fa-remove""></i> Remove</a> " : "") + @"
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
