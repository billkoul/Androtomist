using System;

namespace Androtomist.Models.Database.Entities
{
    public class Process : EntityAbstract
    {
		public Process(long P_ID)
        {
            GetRow(@"
                SELECT 
                    C_PROCESS.P_ID,
                    C_PROCESS.P_NAME,
                    C_PROCESS.P_FILE_ID,
                    C_PROCESS.P_DATE,
                    C_PROCESS.P_TYPE_ID,
                    A_FILE.FILE_NAME AS FILE_NAME
                FROM C_PROCESS
                    INNER JOIN A_FILE ON A_FILE.FILE_ID = C_PROCESS.P_FILE_ID

                WHERE 
                    C_PROCESS.P_ID = " + P_ID + @"
               
                ");
		}

        public bool ExistsProp
        {
            get
            {
                return dataRow != null;
            }
        }

        public long P_ID
        {
            get
            {
                return Convert.ToInt64(dataRow["P_ID"]);

            }
        }
        public long P_TYPE_ID
        {
            get
            {
                return Convert.ToInt64(dataRow["P_TYPE_ID"]);
            }
        }

        public string P_NAME
        {
            get
            {
                return dataRow["P_NAME"].ToString();
            }
        }

        public long P_FILE_ID
        {
            get
            {
                return Convert.ToInt64(dataRow["P_FILE_ID"]);
            }
            set { }
        }

        public DateTime P_DATE
        {
            get
            {
                return Convert.ToDateTime(dataRow["P_DATE"]);
            }
        }


    }
}
