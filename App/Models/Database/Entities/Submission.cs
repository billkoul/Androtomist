using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace Androtomist.Models.Database.Entities
{
    public class Submission : EntityAbstract
    {
		public readonly List<Androtomist.Models.Database.Entities.File> files;
		public Submission(long SUB_ID)
        {
            GetRow(@"
                SELECT 
                    *

                FROM B_SUBS

                WHERE 
                    B_SUBS.SUB_ID = " + SUB_ID + @"
               
                ");

			DataTable dataTable;

			if (SUB_ID > 0)
			{
				dataTable = databaseConnector.SelectSQL("SELECT A_FILE.FILE_ID, B_SUBFILE.SUBFILE_FILE_ID, B_SUBFILE.SUBFILE_SUB_ID FROM A_FILE INNER JOIN B_SUBFILE ON B_SUBFILE.SUBFILE_FILE_ID = FILE_ID WHERE B_SUBFILE.SUBFILE_SUB_ID = " + SUB_ID, "A_FILE");
				files = dataTable.Rows.Cast<DataRow>().Select(x => new File(Convert.ToInt64(x["FILE_ID"]))).ToList();
			}
		}
		public bool ExistsProp
		{
			get
			{
				return dataRow != null;
			}
		}

		public long SUB_ID
		{
            get
            {
                return Convert.ToInt64(dataRow["SUB_ID"]);
            }
        }

		public long SUB_FILE_ID
		{
			get
			{
				return Convert.ToInt64(dataRow["SUB_FILE_ID"]);
			}
		}

		public long SUBS_SCHEMA_ID
		{
			get
			{
				return Convert.ToInt64(dataRow["SUBS_SCHEMA_ID"]);
			}
		}

		public long SUBS_USER_ID
		{
			get
			{
				return Convert.ToInt64(dataRow["SUBS_USER_ID"]);
			}
		}

		public DateTime SUBS_LUD
		{
            get
            {
                return Convert.ToDateTime(dataRow["SUBS_LUD"]);
            }
        }

		public string SUBS_LOG
		{
			get
			{
				return dataRow["SUBS_LOG"].ToString();
			}
		}

	}
}
