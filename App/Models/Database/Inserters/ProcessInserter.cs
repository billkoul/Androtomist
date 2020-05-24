using System;
using System.Collections.Generic;

namespace Androtomist.Models.Database.Inserters
{
    public class ProcessInserter : InserterAbstract
    {
        public string P_ID { get; set; }

        public string P_TYPE_ID { get; set; }
        public string P_NAME { get; set; }
        public string P_FILE_ID { get; set; }

        public DateTime TIME_STAMP_CREATED { get; set; }

        public override long Insert(bool is_insert = false)
        {
            long insert_ID;

            List<string> col_names = new List<string>{
                "P_DATE"
            };

            if (!string.IsNullOrEmpty(P_TYPE_ID)) col_names.Add("P_TYPE_ID");
            if (!string.IsNullOrEmpty(P_NAME)) col_names.Add("P_NAME");
            if (!string.IsNullOrEmpty(P_FILE_ID)) col_names.Add("P_FILE_ID");

            List<string> col_vals = new List<string> {
                "'" + dateNow.ToString("yyyy-MM-dd HH:mm:ss") + "'",
            };

            if (!string.IsNullOrEmpty(P_TYPE_ID)) col_vals.Add("'" + P_TYPE_ID + "'");
            if (!string.IsNullOrEmpty(P_NAME)) col_vals.Add("'" + P_NAME + "'");
            if (!string.IsNullOrEmpty(P_FILE_ID)) col_vals.Add("'" + P_FILE_ID + "'");

            insert_ID = databaseConnector.InsertSQL("C_PROCESS", col_names, col_vals, "P_ID");

            if (insert_ID == 0) throw new Exception("Cannot save process to database.");

            return insert_ID;
        }
    }
}
