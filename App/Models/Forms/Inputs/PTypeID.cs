using System;
using System.Data;

namespace Androtomist.Models.Database.Inputs
{
    public class PTypeID : InputAbstract
    {
        public PTypeID(string extraFilter = "") : base()
        {
            typeAbstract = new TypeSelect("A_PROCESS_TYPE", "PTYPE_NAME", "P_TYPE_ID", extraFilter, string.Empty, null, false, "SELECT * FROM A_PROCESS_TYPE WHERE PTYPE_ID < 3");
            html = typeAbstract.html;
        }

        public override bool Exists()
        {
            return this.html != string.Empty;
        }
    }
}
