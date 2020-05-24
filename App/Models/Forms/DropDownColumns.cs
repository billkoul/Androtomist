using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Androtomist.Models.Forms
{
    public class DropDownColumns
    {
        private readonly string json;

        public DropDownColumns(string json)
        {
            this.json = json;
        }

        public List<FilteredColumn> GetFilteredColumns()
        {
            List<FilteredColumn> SelectColumns = new List<FilteredColumn>();

            string json_str;
            JObject json_obj;
            IList<JToken> jtoken_sections_list;

            if (!string.IsNullOrWhiteSpace(json))
            {
                json_str = HttpUtility.UrlDecode(json);
                json_obj = JObject.Parse(json_str);
                jtoken_sections_list = json_obj["all_columns"].Children().ToList();

                foreach (JToken jtoken_order_list_item in jtoken_sections_list)
                    SelectColumns.Add(JsonConvert.DeserializeObject<FilteredColumn>(jtoken_order_list_item.ToString()));
            }

            return SelectColumns;
        }

        public class FilteredColumn
        {
            public string column_name { get; set; }
            public List<ColumnFilter> filter_vals { get; set; }
        }

        public class ColumnFilter
        {
            public string id { get; set; }
            public string text { get; set; }

            [JsonConverter(typeof(BoolConverter))]
            public bool is_tag { get; set; }
        }
    }
}
