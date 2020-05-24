using System;

namespace Androtomist.Models.Database.Entities
{
    public class Info : EntityAbstract
    {

        public Info(long INFO_ID)
        {
            GetRow(@"SELECT * FROM A_INFO");
        }

		public bool ExistsProp
		{
			get
			{
				return dataRow != null;
			}
		}

		public string TOOLS_PATH
        {
            get
            {
                return dataRow["TOOLS_PATH"].ToString();
            }
        }

        public string PROJECT_PATH
        {
            get
            {
                return dataRow["PROJECT_PATH"].ToString();
            }
        }

        public string SCRIPT
        {
            get
            {
                return dataRow["SCRIPT"].ToString();
            }
        }

        public string FRIDA_PATH
        {
            get
            {
                return dataRow["FRIDA_PATH"].ToString();
            }
        }

        public int EVENTS
        {
            get
            {
                return Convert.ToInt32(dataRow["EVENTS"].ToString());
            }
        }

        public string REMOTE_ADDR
        {
            get
            {
                return dataRow["REMOTE_ADDR"].ToString();
            }
        }
    }
}
