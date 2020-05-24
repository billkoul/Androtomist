namespace Androtomist.Models.Database.Entities
{
    public class Result : EntityAbstract
    {
		public Result(long R_ID)
        {
            GetRow(@"
                    SELECT *
                    FROM C_PROCESS P
                    LEFT JOIN D_RESULTS DI ON DI.R_P_ID = P.P_ID
                    LEFT JOIN R_PERMISSIONS PERM ON PERM.FILE_ID = P.P_FILE_ID
                    LEFT JOIN R_DI DI ON DI.FILE_ID = P.P_FILE_ID
                ");

        }

        public bool ExistsProp
        {
            get
            {
                return dataRow != null;
            }
        }
    }
}
