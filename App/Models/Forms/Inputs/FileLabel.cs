using System;
using System.Data;

namespace Androtomist.Models.Database.Inputs
{
    public class FileLabel : InputAbstract
    {
        public FileLabel(string extraFilter = "") : base()
        {
            typeAbstract = new TypeText("A_FILE", "FILE_LABEL", "FILE_LABEL", extraFilter);
            html = typeAbstract.html;
        }

        public override bool Exists()
        {
            return this.html != string.Empty;
        }
    }
}
