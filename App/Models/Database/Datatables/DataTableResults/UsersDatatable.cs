using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace Androtomist.Models.Database.Datatables
{
    public class UsersDatatable : AbstractDatatable
    {
        public UsersDatatable(DataTableRequest dataTableRequest, long UNIQUE_ID = -1) : base(dataTableRequest, UNIQUE_ID)
        {
            TABLE_TYPE = TABLE_TYPE.USER;

            SQL = @"
                SELECT 
                    B_USER.USER_ID,
                    B_USER.USER_LEVEL_ID,
                    B_USER.USER_NAME,
                    --" + Converters.date_time_format("B_USER.TIME_STAMP_CREATED") + @" AS CREATE_DATETIME,
                    --" + Converters.date_time_format("B_USER.TIME_STAMP_LASTLOG") + @" AS LAST_SEEN_DATETIME,
                    B_USER.TIME_STAMP_CREATED AS CREATE_DATETIME,
                    B_USER.TIME_STAMP_LASTLOG AS LAST_SEEN_DATETIME,
                    B_USER.EMAIL,
                    B_USER.NAME,
                    B_USER.SURNAME,
                    --A_USER_LEVEL.USER_LEVEL_NAME,
                    (CASE WHEN B_USER.ACTIVE = 1 THEN 'YES' ELSE 'NO' END) AS USER_ACTIVE

                FROM B_USER
                    --INNER JOIN A_USER_LEVEL ON A_USER_LEVEL.USER_LEVEL_ID = B_USER.USER_LEVEL_ID
                
                ORDER BY B_USER.TIME_STAMP_CREATED DESC
            ";
        }

        public override DataTableHtml GetDataTableHtml()
        {
            DataTableHtml dataTableHtml = new DataTableHtml
            {
                TABLE_TYPE = TABLE_TYPE,
                UNIQUE_ID = UNIQUE_ID,
                TITLE = "Users",
                ENTITY_NAME = "User",
                DEFAULT_ORDER_COL = 2,
                URL_ADD = "/dashboard/users/add/",               
                COLUMNS = new DataTableHtml.DataColumn[]
                {
                    new DataTableHtml.DataColumn("ACTIONS", "Actions",  false, false),
                    new DataTableHtml.DataColumn("USER_NAME", "Username"),
                    new DataTableHtml.DataColumn("NAME", "Name"),
                    new DataTableHtml.DataColumn("SURNAME", "Surname"),
                    new DataTableHtml.DataColumn("EMAIL", "Email"),
                    new DataTableHtml.DataColumn("CREATE_DATETIME", "Date created"),
                    new DataTableHtml.DataColumn("LAST_SEEN_DATETIME", "Last Seen"),
                    //new DataTableHtml.DataColumn("USER_LEVEL_NAME", "Level"),
                    new DataTableHtml.DataColumn("USER_ACTIVE", "Active"),
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
                                <a class=""dropdown-item"" href=""/dashboard/users/add/" + x["USER_ID"] + @"""><i class=""fa fa-edit""></i> Edit </a> 
                                <div class=""dropdown-divider""></div>
                                " + (dataTableHtml.CAN_CLONE ? @"<a class=""dropdown-item clone-row"" href=""#"" data-row_id=""" + x["USER_ID"] + @"""><i class=""fa fa-clone""></i> Clone</a> " : "") + @"
                                " + (dataTableHtml.CAN_REMOVE ? @"<a class=""dropdown-item delete-row"" href=""#"" data-row_id=""" + x["USER_ID"] + @"""><i class=""fa fa-remove""></i> Remove</a> " : "") + @"
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
