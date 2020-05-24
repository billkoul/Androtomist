using System;
using System.Collections.Generic;
using System.Data;
using Androtomist.Models.Database.Inserters;
using Androtomist.Models.Forms;
using Androtomist.Models.Database.User;

namespace Androtomist.Models.Database.Helpers
{
    public class UserHelper : AbstractHelper
    {
		public Entities.User AddUser(string createJson)
		{
			FilterData filterData = new FilterData(createJson);

			long.TryParse(filterData.user_id, out long USER_ID);
			if (string.IsNullOrEmpty(filterData.USR_USER_LEVEL)) throw new FormDataException("Please add user level for the new user.");
			if (string.IsNullOrEmpty(filterData.USR_NAME)) throw new FormDataException("Please add a name for the new user.");
			if (string.IsNullOrEmpty(filterData.USR_SURNAME)) throw new FormDataException("Please add a surname for the new user.");
			if (string.IsNullOrEmpty(filterData.USR_USER_NAME)) throw new FormDataException("Please add a username for the new user.");
			if (string.IsNullOrEmpty(filterData.USR_PASSWORD) || filterData.USR_PASSWORD.Length < 6) throw new FormDataException("Password must be at least 6 characters.");
			if (string.IsNullOrEmpty(filterData.USR_EMAIL) || !IsValidEmail(filterData.USR_EMAIL)) throw new FormDataException("Please add a valid email for the new user.");


            string preparedPassword = SHA.GenerateSHA512String(filterData.USR_PASSWORD);

            UserInserter userInserter = new UserInserter
			{
				USER_ID = (USER_ID < 0 ? null : USER_ID.ToString()),
				NAME = filterData.USR_NAME,
				SURNAME = filterData.USR_SURNAME,
				USERNAME = filterData.USR_USER_NAME,
				PASSWORD = preparedPassword,
				EMAIL = filterData.USR_EMAIL,
				USERLEVEL = ((int)Enum.Parse(typeof(USER_LEVEL), filterData.USR_USER_LEVEL)).ToString()
            };
			if (USER_ID > 0) userInserter.Insert();
			else USER_ID = userInserter.Insert(true);

			Entities.User user = new Entities.User(USER_ID);
			if (!user.Exists()) throw new Exception("Cannot add user to database");

			return user;
		}

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void RemoveUsers(List<long> USER_IDS)
        {
            int rowsAffected = 0;

            DataTable dataTable;
            foreach (string TableName in new string[] { "D_FILE", "D_BASELINE", "D_BASELINE_UPLOAD", "D_CHART", "D_LAYER", "D_SCENARIO", "D_CHANGE" })
            {
                dataTable = databaseConnector.SelectSQL("SELECT COUNT(USER_ID) AS USERS FROM " + TableName + " WHERE USER_ID IN( " + string.Join(",", USER_IDS) + ")", TableName);
                if (dataTable.Columns.Count != 0 &&
                    dataTable.Rows.Count != 0 &&
                    dataTable.Rows[0]["USERS"] != DBNull.Value &&
                    (decimal)dataTable.Rows[0]["USERS"] > 0)

                    throw new Exception("Could not remove this user.<br />Please remove all data first.");
            }

            rowsAffected = databaseConnector.DeleteSQL("DELETE FROM B_USER WHERE USER_ID IN(" + string.Join(",", USER_IDS) + ")");

            if (rowsAffected == 0)
                throw new Exception("Could not remove any records from database.");

            if (rowsAffected != USER_IDS.Count)
                throw new Exception("Could not remove " + (USER_IDS.Count - rowsAffected) + " records from database.");
        }

        public void LastSeen()
        {
            StringFuncs stringFuncs = new StringFuncs();
            long rowsAffected;

            List<string> col_names = new List<string> { "TIME_STAMP_LASTLOG" };
            List<string> col_vals = new List<string> { DateTime.Now.ToString()};
            rowsAffected = databaseConnector.UpdateSQL("B_USER", col_names, col_vals, "USER_ID = " + UserId.ToString());
            //if (rowsAffected == 0) throw new Exception("Cannot save user [" + USER_ID + "] to database.");
        }
    }


}