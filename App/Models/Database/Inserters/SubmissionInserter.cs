using System;
using System.Collections.Generic;

namespace Androtomist.Models.Database.Inserters
{
    public class SubmissionInserter : InserterAbstract
    {
        public string SUB_ID { get; set; }
		public string SUB_FILE_ID { get; set; }
		public string SUBS_SCHEMA_ID { get; set; }
		public string SUBS_LOG { get; set; }

		public override long Insert(bool is_insert = false)
        {
            long insert_ID;

            List<string> col_names = new List<string>
            {
				"SUBS_SCHEMA_ID",
				"SUBS_USER_ID",
				"SUBS_LUD",
				"SUBS_LOG"
			};

			List<string> col_vals = new List<string>
            {
				"'" + SUBS_SCHEMA_ID + "'",
				"'" + UserId + "'",
				"'" + dateNow.ToString("yyyy-MM-dd HH:mm:ss") + "'",
				"'" + SUBS_LOG + "'"
			};

			insert_ID = (is_insert ? databaseConnector.InsertSQL("B_SUBS", col_names, col_vals, "SUB_ID") : databaseConnector.UpdateSQL("B_SUBS", col_names, col_vals, "SUB_ID = " + SUB_ID.ToString()));

            if (insert_ID == 0) throw new Exception("Cannot add submission.");

            return insert_ID;
        }

    }
}
