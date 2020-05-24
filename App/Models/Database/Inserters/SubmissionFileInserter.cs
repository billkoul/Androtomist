using System;
using System.Collections.Generic;

namespace Androtomist.Models.Database.Inserters
{
    public class SubmissionFileInserter : InserterAbstract
    {
        public string SUBFILE_ID { get; set; }
		public string SUBFILE_SUB_ID { get; set; }
		public string SUBFILE_FILE_ID { get; set; }

		public override long Insert(bool is_insert = false)
        {
            long insert_ID;

            List<string> col_names = new List<string>
            {
				"SUBFILE_SUB_ID",
				"SUBFILE_FILE_ID"
			};

			List<string> col_vals = new List<string>
            {
				"'" + SUBFILE_SUB_ID + "'",
				"'" + SUBFILE_FILE_ID + "'"
			};

			insert_ID = (is_insert ? databaseConnector.InsertSQL("B_SUBFILE", col_names, col_vals, "SUBFILE_ID") : databaseConnector.UpdateSQL("B_SUBFILE", col_names, col_vals, "SUBFILE_ID = " + SUBFILE_ID.ToString()));

            if (insert_ID == 0) throw new Exception("Cannot save file submission to database.");

            return insert_ID;
        }

    }
}
