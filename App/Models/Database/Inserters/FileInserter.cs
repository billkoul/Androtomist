using System;
using System.Collections.Generic;

namespace Androtomist.Models.Database.Inserters
{
    public class FileInserter : InserterAbstract
    {
        public string FILE_ID { get; set; }
		public string FILE_NAME { get; set; }
		public string FILE_PATH { get; set; }
		public string FILE_SIZE { get; set; }
		public string FILE_LABEL { get; set; }
		public string FILE_RELATIVE_PATH { get; set; }

		public override long Insert(bool is_insert = false)
        {
            long insert_ID;

            List<string> col_names = new List<string>
            {
				"FILE_LUD",
				"FILE_USER_ID",
				"FILE_PATH",
				"FILE_SIZE",
				"FILE_LABEL",
				"FILE_RELATIVE_PATH"
			};
			if (!string.IsNullOrEmpty(FILE_NAME)) col_names.Add("FILE_NAME");

			List<string> col_vals = new List<string>
            {
                "'" + dateNow.ToString("yyyy-MM-dd HH:mm:ss") + "'",
				"'" + UserId + "'",
				"'" + FILE_PATH + "'",
				"'" + FILE_SIZE + "'",
				"'" + FILE_LABEL + "'",
				"'" + FILE_RELATIVE_PATH + "'"
			};
			if (!string.IsNullOrEmpty(FILE_NAME)) col_vals.Add("'" + FILE_NAME + "'");


			insert_ID = (is_insert ? databaseConnector.InsertSQL("A_FILE", col_names, col_vals, "FILE_ID") : databaseConnector.UpdateSQL("A_FILE", col_names, col_vals, "FILE_ID = " + FILE_ID.ToString()));

            if (insert_ID == 0) throw new Exception("Cannot save file to database.");

            return insert_ID;
        }

    }
}
