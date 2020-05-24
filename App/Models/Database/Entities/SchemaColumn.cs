using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace Androtomist.Models.Database.Entities
{
    public class SchemaColumn : EntityAbstract
    {
		public readonly List<Androtomist.Models.Database.Entities.SchemaColumn> columns;
		public SchemaColumn(long ID, long SCHEMACOL_SCHEMA_ID = -1)
        {
            GetRow(@"
                SELECT 
                    *

                FROM A_SCHEMA_COLUMNS

                WHERE A_SCHEMA_COLUMNS.SCHEMACOL_ID = " + ID
			);

			DataTable dataTable;

			if (SCHEMACOL_SCHEMA_ID > 0)
			{
				dataTable = databaseConnector.SelectSQL("SELECT * FROM A_SCHEMA_COLUMNS WHERE A_SCHEMA_COLUMNS.SCHEMACOL_SCHEMA_ID = " + SCHEMACOL_SCHEMA_ID, "A_SCHEMA_COLUMNS");
				columns = dataTable.Rows.Cast<DataRow>().Select(x => new SchemaColumn(Convert.ToInt64(x["SCHEMACOL_ID"]))).ToList();
			}
		}

		public bool ExistsProp
		{
			get
			{
				return dataRow != null;
			}
		}

		public long SCHEMACOL_ID
		{
			get
			{
				return Convert.ToInt64(dataRow["SCHEMACOL_ID"]);
			}
		}
		public long SCHEMACOL_SCHEMA_ID
		{
            get
            {
				return Convert.ToInt64(dataRow["SCHEMACOL_SCHEMA_ID"]);
			}
        }
		public string SCHEMACOL_STARTSWITH
		{
			get
			{
				return dataRow["SCHEMACOL_STARTSWITH"].ToString();
			}
		}

		public string SCHEMACOL_TABLE
		{
			get
			{
				return dataRow["SCHEMACOL_TABLE"].ToString();
			}
		}

		public string SCHEMACOL_COLUMN
		{
			get
			{
				return dataRow["SCHEMACOL_COLUMN"].ToString();
			}
		}

		public string SCHEMACOL_NULLABLE
		{
			get
			{
				return dataRow["SCHEMACOL_NULLABLE"].ToString();
			}
		}

		public string SCHEMACOL_DATATYPE
		{
			get
			{
				return dataRow["SCHEMACOL_DATATYPE"].ToString();
			}
		}

		public string SCHEMACOL_PRECISION
		{
			get
			{
				return dataRow["SCHEMACOL_PRECISION"].ToString();
			}
		}

		public string SCHEMACOL_FORMAT
		{
			get
			{
				return dataRow["SCHEMACOL_FORMAT"].ToString();
			}
		}

	}
}
