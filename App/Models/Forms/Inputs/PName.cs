using System;
using System.Data;

namespace Androtomist.Models.Database.Inputs
{
    public class PName : InputAbstract
    {
        public PName(string extraFilter = "") : base()
        {
            typeAbstract = new TypeText("C_PROCESS", "P_NAME", "P_NAME", extraFilter);
            html = typeAbstract.html;
        }

        public override bool Exists()
        {
            return this.html != string.Empty;
        }
    }
}
