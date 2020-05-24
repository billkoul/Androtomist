using System;
using System.Data;

namespace Androtomist.Models.Database.Inputs
{
    public class SubID : InputAbstract
    {
        public SubID(string extraFilter = "") : base()
        {
            typeAbstract = new TypeUpload("A_FILE", "FILE_ID", "UPLOAD_ID", extraFilter);
            html = typeAbstract.html;
        }

        public override bool Exists()
        {
            return this.html != string.Empty;
        }
    }
}
