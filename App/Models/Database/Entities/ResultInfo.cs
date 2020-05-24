using System.Data;
using System.Text.RegularExpressions;

namespace Androtomist.Models.Database.Entities
{
    public class ResultInfo : EntityAbstract
    {
        public long FILE_ID;
        public bool isExport = false;

        public ResultInfo(long FILE_ID)
        {
            this.FILE_ID = FILE_ID;
        }

        public bool ExistsProp
        {
            get
            {
                return dataRow != null;
            }
        }

        public bool PROCESSED
        {
            get
            {
                GetTable(@"
                    SELECT 
                        *
                    FROM C_PROCESS
                    WHERE C_PROCESS.P_FILE_ID = " + FILE_ID);

                return this.dataTable?.Rows.Count > 0 ? true : false;
            }
        }

        public string TYPE
        {
            get
            {
                GetTable(@"
                    SELECT 
                        case
                            WHEN P_TYPE_ID = 3 THEN 'TRAINING'
                            WHEN P_TYPE_ID = 0 THEN 'STATIC'
                            WHEN P_TYPE_ID = 1 THEN 'DYNAMIC'
                            WHEN P_TYPE_ID = 2 THEN 'HYBRID'
                            ELSE ''
                        END AS TYPE
                    FROM C_PROCESS
                    WHERE C_PROCESS.P_FILE_ID = " + FILE_ID);

                return this.dataTable?.Rows.Count > 0 ? this.dataTable.Rows[0][0].ToString() : "";
            }
        }

        public DataTable INFO
        {
            get
            {
                GetTable(@"
                    SELECT 
                        A_FILE.FILE_NAME,
                        A_FILE.FILEHASH,
                        A_FILE.PACKAGE_NAME
                    FROM A_FILE
                    WHERE A_FILE.FILE_ID = " + FILE_ID);

                return this.dataTable;
            }
        }

        public int WARNING
        {
            get
            {
                GetTable(@"
                    SELECT
                        COALESCE(MAX(INTENT_WARNING + PERMISSION_WARNING + API_CALL_WARNING),0) AS WARNING
                        FROM (
                        SELECT 
	                        CASE
		                        WHEN V_I_COMB.INTENT IS NOT NULL THEN 1
		                        ELSE 0
	                        END AS INTENT_WARNING,
	                        CASE
		                        WHEN V_P_COMB.PERMISSION IS NOT NULL THEN 1
		                        ELSE 0
	                        END AS PERMISSION_WARNING,
	                        CASE
		                        WHEN V_API_COMB.API_CALL IS NOT NULL THEN 1
		                        ELSE 0
	                        END AS API_CALL_WARNING,
		                        CASE
			                        WHEN V_TRAFFIC_COMB.TRAFFIC IS NOT NULL THEN 1
			                        ELSE 0
		                        END AS TRAFFIC_WARNING
	                        FROM A_FILE
	                        LEFT JOIN R_INTENT I ON I.FILE_ID = A_FILE.FILE_ID
	                        LEFT JOIN R_PERMISSIONS PERM ON PERM.FILE_ID = A_FILE.FILE_ID
	                        LEFT JOIN R_API_CALLS API ON API.FILE_ID = A_FILE.FILE_ID
	                        LEFT JOIN A_TRAIN_DI DI ON DI.FILE_ID = A_FILE.FILE_ID
	                        LEFT JOIN V_P_COMB ON V_P_COMB.PERMISSION = PERM.PERMISSION
	                        LEFT JOIN V_I_COMB ON V_I_COMB.INTENT = I.INTENT
	                        LEFT JOIN V_API_COMB ON V_API_COMB.API_CALL = API.API_CALL
	                        LEFT JOIN V_TRAFFIC_COMB ON V_TRAFFIC_COMB.TRAFFIC = DI.TRAFFIC
                            WHERE A_FILE.FILE_ID = " + FILE_ID + ") T");

                return this.dataTable?.Rows.Count > 0 ? int.Parse(dataTable.Rows[0]["WARNING"].ToString()) : 0;
            }
        }

        public DataTable PERMISSIONS
        {
            get
            {
                GetTable(@"
                    SELECT 
                        --A_FILE.FILE_NAME,
                        --A_FILE.FILEHASH,
                        --A_FILE.PACKAGE_NAME,
                        CASE
                            WHEN V_P_COMB.PERMISSION IS NOT NULL THEN " + (!isExport ? "'<span class=''warning''>' || " : "") + "PERM.PERMISSION" + (!isExport ? " || '</span>'" : " ") + @"
                            ELSE PERM.PERMISSION
                        END AS PERMISSION
                    FROM A_FILE
                    LEFT JOIN R_PERMISSIONS PERM ON PERM.FILE_ID = A_FILE.FILE_ID
                    LEFT JOIN V_P_COMB ON V_P_COMB.PERMISSION = PERM.PERMISSION
                    WHERE PERM.PERMISSION LIKE '%permission.%' 
                    AND A_FILE.FILE_ID = " + FILE_ID);

                return this.dataTable;
            }
        }

        public DataTable INTENT
        {
            get
            {
                GetTable(@"
                    SELECT 
                        --A_FILE.FILE_NAME,
                        --A_FILE.FILEHASH,
                        --A_FILE.PACKAGE_NAME,
                        CASE
                            WHEN V_I_COMB.INTENT IS NOT NULL THEN " + (!isExport ? "'<span class=''warning''>' || " : " ") + "I.INTENT" + (!isExport ? " || '</span>'" : " ") + @"
                            ELSE I.INTENT
                        END AS INTENT
                    FROM A_FILE
                    LEFT JOIN R_INTENT I ON I.FILE_ID = A_FILE.FILE_ID
                    LEFT JOIN V_I_COMB ON V_I_COMB.INTENT = I.INTENT
                    WHERE I.INTENT LIKE '%intent%'
                    AND A_FILE.FILE_ID = " + FILE_ID);

                return this.dataTable;
            }
        }

        public DataTable API
        {
            get
            {
                GetTable(@"
                    SELECT 
                        CASE
                            WHEN V_API_COMB.API_CALL IS NOT NULL THEN '<span class=''warning''>' || API.API_CALL || '</span>'
                            ELSE API.API_CALL
                        END AS API_CALL
                    FROM A_FILE
                    LEFT JOIN R_API_CALLS API ON API.FILE_ID = A_FILE.FILE_ID
                    LEFT JOIN V_API_COMB ON V_API_COMB.API_CALL = API.API_CALL
                    WHERE A_FILE.FILE_ID = " + FILE_ID);

                return this.dataTable;
            }
        }

        public DataTable DYNAMIC
        {
            get
            {
                DataTable DiDT = databaseConnector.SelectSQL(@"
                        SELECT 
	                        '' AS TYPE,
	                        '' AS DETAILS
                        FROM A_FILE WHERE 0=1", "");

                GetTable(@"
                    SELECT 
                        DI.TRAFFIC
                    FROM A_FILE
                    LEFT JOIN A_TRAIN_DI DI ON DI.FILE_ID = A_FILE.FILE_ID
                    WHERE A_FILE.FILE_ID = " + FILE_ID);

                if (this.dataTable?.Rows.Count > 0)
                {
                    string traffic = dataTable.Rows[0]["TRAFFIC"].ToString().ToLower();

                    int socket = Regex.Matches(traffic, "java.net.socket").Count;
                    if(socket > 0)
                    {
                        DataRow row = DiDT.NewRow();
                        row["TYPE"] = "java.net.Socket";
                        row["DETAILS"] = "Using socket connection";
                        DiDT.Rows.Add(row);
                    }
           
                    int url = Regex.Matches(traffic, "java.net.url").Count;
                    if (url > 0)
                    {
                        DataRow row = DiDT.NewRow();
                        row["TYPE"] = "java.net.URL";
                        row["DETAILS"] = "Communicating via http";
                        DiDT.Rows.Add(row);
                    }

                    MatchCollection ipMatches = Regex.Matches(traffic, @"\b(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\b");
                    foreach(var match in ipMatches)
                    {
                        DataRow row = DiDT.NewRow();
                        row["TYPE"] = "Detected traffic";
                        row["DETAILS"] = "Target: " + match.ToString();
                        DiDT.Rows.Add(row);
                    }
                }

                return DiDT;
            }
        }
    }
}
