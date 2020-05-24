using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Androtomist.Models.Database;

namespace Androtomist.Models.Forms
{
    public class FilterData : DBClass
    {
        private List<JSONVal> JSONVals { get; set; }

        //ENTITY ID
        public string user_id { get; set; }
        public string upload_id { get; set; }
		public string schema_id { get; set; }

        public string[] id { get; set; }
        public string[] SCHEMA_STARTS_WITH { get; set; }
		public string[] SCHEMA_COLUMNS { get; set; }
		public string[] SCHEMA_TABLE { get; set; }
		public string[] SCHEMA_NULLABLE { get; set; }
		public string[] SCHEMA_DATATYPE { get; set; }
		public string[] SCHEMA_PRECISION { get; set; }
		public string[] SCHEMA_FORMAT { get; set; }

		//USER
		public string USR_NAME { get; set; }
        public string USR_SURNAME { get; set; }
        public string USR_USER_NAME { get; set; }
        public string USR_PASSWORD { get; set; }
        public string USR_EMAIL { get; set; }
        public string USR_USER_LEVEL { get; set; }

		//File
		public string FILE_ID { get; set; }
		public string UPLOAD_ID { get; set; }
		public string FILE_LABEL { get; set; }

        //Process
        public string P_ID { get; set; }
        public string P_NAME { get; set; }
        public string P_TYPE_ID { get; set; }

        //Results
        public string R_ID { get; set; }
        public string R_NAME { get; set; }

        // Constructor
        public FilterData(string jsonFilter)
        {
            if (string.IsNullOrEmpty(jsonFilter)) throw new System.Exception("Could not get any form data.");

            Global.ColorFuncs colorFuncs = new Global.ColorFuncs();

            JSONVals = JsonConvert.DeserializeObject<List<JSONVal>>(jsonFilter);

            PropertyInfo[] props = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (PropertyInfo propertyInfo in props)
            {
                List<string> vals = JSONVals.Where(x => x.Name.ToLower().Contains(propertyInfo.Name.ToLower())).DefaultIfEmpty(new JSONVal()).Select(x => x.Value).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

                if (propertyInfo.PropertyType == typeof(System.Drawing.Color))
                {
                    propertyInfo.SetValue(this, System.Drawing.Color.Black, null);
                    if (vals.Count > 0) propertyInfo.SetValue(this, colorFuncs.RGBstringToColor(vals.First()), null);
                }
                else if (propertyInfo.PropertyType == typeof(bool))
                {
                    propertyInfo.SetValue(this, false, null);
                    if (vals.Count > 0) propertyInfo.SetValue(this, vals.First().ToLower().Equals("on"), null); 
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    propertyInfo.SetValue(this, string.Empty, null);
                    if (vals.Count > 0) propertyInfo.SetValue(this, vals.First(), null);
                }
				else if (propertyInfo.PropertyType == typeof(string[]))
				{
					//propertyInfo.SetValue(this, string.Empty, null);
					if (vals.Count > 0)
					{
						for(int i=0;i<vals.Count;i++)
							propertyInfo.SetValue(this, vals.ToArray<string>(), null);
					}
				}
				else if (propertyInfo.PropertyType == typeof(List<System.Drawing.Color>))
                {
                    propertyInfo.SetValue(this, new List<System.Drawing.Color>(), null);
                    if (vals.Count > 0) propertyInfo.SetValue(this, vals.Select(x => colorFuncs.RGBstringToColor(x)).ToList(), null);
                }
                else if (propertyInfo.PropertyType == typeof(List<bool>))
                {
                    propertyInfo.SetValue(this, new List<bool>(), null);
                    if (vals.Count > 0) propertyInfo.SetValue(this, vals.Select(x => x.ToLower().Equals("on")).ToList(), null); 
                }
                else if (propertyInfo.PropertyType == typeof(List<string>))
                {
                    propertyInfo.SetValue(this, new List<string>(), null);
                    if (vals.Count > 0) propertyInfo.SetValue(this, vals.ToList(), null);
                }
            }
        }

        public class JSONVal
        {
            public JSONVal()
            {
                Name = string.Empty;
                Value = string.Empty;
            }

            public string Name { get; set; }
            public string Value { get; set; }
        }

        public List<JSONVal> getJSONVals
        {
            get
            {
                return JSONVals;
            }
        }

    }
}
