using System;
using System.Data;

namespace Androtomist.Models.Database.Inputs
{
    public class FileID : InputAbstract
    {
        public FileID(string extraFilter = "") : base()
        {
            typeAbstract = new TypeSelect("A_FILE", "FILE_NAME", "P_FILE_ID", extraFilter, string.Empty, null, false, "SELECT FILE_ID, FILE_NAME FROM A_FILE WHERE IS_SAMPLE IS NULL");
            html = typeAbstract.html;
        }

        public override bool Exists()
        {
            return this.html != string.Empty;
        }
    }
}
