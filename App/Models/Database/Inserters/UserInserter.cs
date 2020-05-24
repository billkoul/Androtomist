using System;
using System.Collections.Generic;

namespace Androtomist.Models.Database.Inserters
{
    public class UserInserter : InserterAbstract
    {
        public string USER_ID { get; set; }
        public string NAME { get; set; }
        public string SURNAME { get; set; }
        public string EMAIL { get; set; }
        public string PASSWORD { get; set; }
        public string USERNAME { get; set; }
        public string USERLEVEL { get; set; }

        public DateTime TIME_STAMP_CREATED { get; set; }

        public override long Insert(bool is_insert = false)
        {
            long insert_ID;

            List<string> col_names = new List<string>
            {
                "TIME_STAMP_CREATED",
                "TIME_STAMP_LASTLOG",
                "ACTIVE",
                "USER_LEVEL_ID"
            };
            if (!string.IsNullOrEmpty(NAME)) col_names.Add("NAME");
            if (!string.IsNullOrEmpty(SURNAME)) col_names.Add("SURNAME");
            if (!string.IsNullOrEmpty(USERNAME)) col_names.Add("USER_NAME");
            if (!string.IsNullOrEmpty(EMAIL)) col_names.Add("EMAIL");
            if (!string.IsNullOrEmpty(PASSWORD)) col_names.Add("PASSWORD");

            List<string> col_vals = new List<string>
            {
                "'" + dateNow.ToString("yyyy-MM-dd HH:mm:ss") + "'",
                "'" + dateNow.ToString("yyyy-MM-dd HH:mm:ss") + "'",
            "1"
            };
            if (!string.IsNullOrEmpty(USERLEVEL)) col_vals.Add("" + USERLEVEL + "");
            if (!string.IsNullOrEmpty(NAME)) col_vals.Add("'" + NAME + "'");
            if (!string.IsNullOrEmpty(SURNAME)) col_vals.Add("'" + EMAIL + "'");
            if (!string.IsNullOrEmpty(USERNAME)) col_vals.Add("'" + USERNAME + "'");
            if (!string.IsNullOrEmpty(EMAIL)) col_vals.Add("'" + EMAIL + "'");
            if (!string.IsNullOrEmpty(PASSWORD)) col_vals.Add("'" + PASSWORD + "'");

            insert_ID = (is_insert ? databaseConnector.InsertSQL("B_USER", col_names, col_vals, "USER_ID") : databaseConnector.UpdateSQL("B_USER", col_names, col_vals, "USER_ID = " + USER_ID.ToString()));

            if (insert_ID == 0) throw new Exception("Cannot save user to database.");

            return insert_ID;
        }
    }
}
