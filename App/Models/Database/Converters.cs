using System;

namespace Androtomist.Models.Database
{
    public class Converters
    {
        public string date_time_format(string column_name)
        {
            return "TO_CHAR(" + column_name + ", '" + datetime_format_str_SQL + "', 'nls_date_language = AMERICAN')";
        }


        public string datetime_format_str_SQL
        {
            get
            {
                return "DD MON YYYY, HH24:MI:SS";
            }
        }
    }
}
