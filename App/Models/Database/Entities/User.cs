using System;

namespace Androtomist.Models.Database.Entities
{
    public class User : EntityAbstract
    {
        public User(long USER_ID)
        {
            GetRow(@"
                SELECT 
                    B_USER.USER_ID,
                    B_USER.USER_LEVEL_ID,
                    B_USER.TIME_STAMP_CREATED,
                    B_USER.TIME_STAMP_LASTLOG,
                    B_USER.USER_NAME,
                    B_USER.EMAIL,
                    B_USER.PASSWORD,
                    B_USER.NAME,
                    B_USER.SURNAME,
                    B_USER.ACTIVE

                FROM B_USER

                WHERE 
                    B_USER.USER_ID = " + USER_ID + @"
               
                ");
        }

		public bool ExistsProp
		{
			get
			{
				return dataRow != null;
			}
		}

		public long USER_ID
        {
            get
            {
                return Convert.ToInt64(dataRow["USER_ID"]);
            }
        }

        public string USER_NAME
        {
            get
            {
                return Convert.ToString(dataRow["USER_NAME"]);
            }
        }

        public string EMAIL
        {
            get
            {
                return Convert.ToString(dataRow["EMAIL"]);
            }
        }

        public string NAME
        {
            get
            {
                return Convert.ToString(dataRow["NAME"]);
            }
        }

        public string SURNAME
        {
            get
            {
                return Convert.ToString(dataRow["SURNAME"]);
            }
        }

        public string PASSWORD
        {
            get
            {
                return Convert.ToString(dataRow["PASSWORD"]);
            }
        }

        public USER_LEVEL USER_LEVEL
        {
            get
            {
                int USER_LEVEL_ID = Convert.ToInt32(dataRow["USER_LEVEL_ID"]);
                if (Enum.IsDefined(typeof(USER_LEVEL), USER_LEVEL_ID))
                {
                    return (USER_LEVEL)USER_LEVEL_ID;
                }
                else
                {
                    throw new Exception("This user level does not exists in database.");
                }
            }
        }

        public DateTime DATE_CREATED
        {
            get
            {
                return Convert.ToDateTime(dataRow["TIME_STAMP_CREATED"]);
            }
        }

        public DateTime LAST_DATE_LOGED
        {
            get
            {
                return Convert.ToDateTime(dataRow["TIME_STAMP_LASTLOG"]);
            }
        }

        public bool IS_ACTIVE
        {
            get
            {
                return Convert.ToInt32(dataRow["ACTIVE"]) == 1;
            }
        }

        public bool HAS_CHANGES
        {
            get
            {
                return !LAST_DATE_LOGED.Equals(DATE_CREATED);
            }
        }

    }
}
