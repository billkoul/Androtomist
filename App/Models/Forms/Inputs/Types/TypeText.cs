using System;
using System.Data;
using Androtomist.Models.Forms;

namespace Androtomist.Models.Database.Inputs
{
    public class TypeText : TypeAbstract
    {
        public TypeText(string tableName = "", string column = "", string name = "", string current = "", string fitlerWith = "", Dependable d = null, bool is_parent = false, string placeholder = "") : base()
        {
            this.name = name;
            this.current = current;
			this.d = d;
			this.is_parent = is_parent;
			this.placeholder = placeholder;

			Populate(string.Empty, string.Empty);
        }

        public override void Populate(string SQL, string tableName)
        {
			string extra_class = "";
			if (d != null)
			{
				foreach (string[,] input in d.inputs)
				{
					extra_class += " depend_" + input[0, 0] + "_" + input[1, 0];
				}
			}
			extra_class += (is_parent ? " parent" : "");

			html = "<input type='text' " + (name.Contains("SCHEMA_S") ? "" : "class='form-control m-input") + "" + extra_class + " id='" + name + "' name='" + name + "' value='" + current + "' placeholder='"+ placeholder+"' />";
        }
    }
}
