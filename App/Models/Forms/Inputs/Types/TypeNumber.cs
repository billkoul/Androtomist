using System;
using System.Data;
using Androtomist.Models.Forms;

namespace Androtomist.Models.Database.Inputs
{
    public class TypeNumber : TypeAbstract
    {
        public TypeNumber(string tableName = "", string column = "", string name = "", string current = "", string fitlerWith = "", int min = 0, int max = 10, int step = 1, Dependable d = null, bool is_parent = false) : base()
        {
            this.name = name;
            this.current = current;
            this.min = min;
            this.max = max;
            this.step = step;
			this.d = d;
			this.is_parent = is_parent;

			Populate(string.Empty, string.Empty);
        }

        public override void Populate(string SQL, string tableName)
        {
            html = "<input type='number' class='form-control m-input' id='" + name + "' name='" + name + "' value='" + current + "' min='" + min  + "' max='" + max + "' step='" + step + "' />";
        }
    }
}
