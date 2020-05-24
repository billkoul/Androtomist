using System;

namespace Androtomist.Models.Database.Entities
{
    public class Schema : EntityAbstract
    {
        public Schema(long ID)
        {
            GetRow(@"
                SELECT 
                    *

                FROM A_SCHEMA

                WHERE 
                    A_SCHEMA.SCHEMA_ID = " + ID + @"
               
                ");
        }

		public bool ExistsProp
		{
			get
			{
				return dataRow != null;
			}
		}

		public long SCHEMA_ID
		{
			get
			{
				return Convert.ToInt64(dataRow["SCHEMA_ID"]);
			}
		}
		public long SCHEMA_USER_ID
		{
            get
            {
				return Convert.ToInt64(dataRow["SCHEMA_USER_ID"]);
			}
        }
		public string SCHEMA_NAME
		{
			get
			{
				return dataRow["SCHEMA_NAME"].ToString();
			}
		}

		public string SCHEMA
		{
            get
            {
                return dataRow["SCHEMA"].ToString();
            }
        }

		public string SCHEMA_STARTS_WITH
		{
			get
			{
				return string.Empty;
			}
		}

		public string SCHEMA_SFORMAT
		{
			get
			{
				return string.Empty;
			}
		}
		public string SCHEMA_STYPE
		{
			get
			{
				return string.Empty;
			}
		}

		public string SCHEMA_COLUMNS
		{
			get
			{
				return string.Empty;
			}
		}

		public DateTime SCHEMA_LUD
		{
			get
			{
				return Convert.ToDateTime(dataRow["SCHEMA_LUD"].ToString());
			}
		}

	}
}
