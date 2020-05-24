using System;
using System.Data;
using Androtomist.Models.Forms;

namespace Androtomist.Models.Database.Inputs
{
    public class TypeDateTime : TypeAbstract
    {
        public TypeDateTime(string tableName = "", string column = "", string name = "", string current = "", string fitlerWith = "", Dependable d = null, bool is_parent = false) : base()
        {
			this.tableName = tableName;
			this.column = column;
			this.name = name;
			this.fitlerWith = fitlerWith;
			this.current = current;
			this.d = d;
			this.is_parent = is_parent;

			Populate(string.Empty, string.Empty);
        }

        public override void Populate(string SQL, string tableName)
        {
			DateTime dateT = (DateTime.TryParse(current, out dateT) ? dateT : DateTime.Now);
			html = "<input type='datetime-local' class='form-control m-input' id='" + name + "' name='" + name + "' value='" + dateT.ToString("yyyy-MM-dd HH:mm:ss").Replace(' ', 'T') + "' />";
        }
}
}
