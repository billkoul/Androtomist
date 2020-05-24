using System;
using System.Collections.Generic;

namespace Androtomist.Models.Database.Inserters
{
    public class ResultsInserter : InserterAbstract
    {
        public string R_ID { get; set; }
        public string R_NAME { get; set; }

        public DateTime TIME_STAMP_CREATED { get; set; }

        public override long Insert(bool is_insert = false)
        {
            long insert_ID;

            List<string> col_names = new List<string>
            {
                "TIME_STAMP_CREATED",
            };
            if (!string.IsNullOrEmpty(R_NAME)) col_names.Add("R_NAME");

            List<string> col_vals = new List<string>
            {
                "'" + dateNow.ToString("yyyy-MM-dd HH:mm:ss") + "'",
            };
            if (!string.IsNullOrEmpty(R_NAME)) col_vals.Add("" + R_NAME + "");

            insert_ID = (is_insert ? databaseConnector.InsertSQL("D_RESULTS", col_names, col_vals, "R_ID") : databaseConnector.UpdateSQL("D_RESULTS", col_names, col_vals, "R_ID = " + R_ID.ToString()));

            if (insert_ID == 0) throw new Exception("Cannot save results database.");

            return insert_ID;
        }
    }
}
