using System;
using System.Text.RegularExpressions;
using System.Data;

namespace Androtomist.Models.Database.User
{
    public class UserLoginValidator : DBClass
    {
        private DataTable dataTable;

        public UserLoginValidator(string username, string password)
        {
            string SQL;

            string preparedUname = username;
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            preparedUname = rgx.Replace(preparedUname, "");
            string preparedPassword = SHA.GenerateSHA512String(password);

            SQL = @"
                SELECT 
                    * 

                FROM 
                    B_USER
                
                WHERE 
                    USER_NAME LIKE '" + preparedUname + @"' 
                    AND UPPER(PASSWORD) LIKE '" + preparedPassword + @"'
            ";

            dataTable = databaseConnector.SelectSQL(SQL, "B_USER");
        }

        public bool CheckCredentials()
        {
            return dataTable != null && dataTable.Rows.Count == 1;
        }

        public long UserID
        {
            get
            {
                return Convert.ToInt64(dataTable.Rows[0]["USER_ID"]);
            }
        }

        public string Name
        {
            get
            {
                return dataTable.Rows[0]["NAME"].ToString();
            }
        }

        public string SurName
        {
            get
            {
                return dataTable.Rows[0]["SURNAME"].ToString();
            }
        }
    }
}
