using System;
using System.Data;

namespace Androtomist.Models.Database.Inputs
{
    public abstract class InputAbstract : DBClass
    {
        public string html { get; set; }
        protected string ExtraFilter { get; set; }
        public TypeAbstract typeAbstract { get; set; }

        public InputAbstract(string extraFilter = "")
        {
            html = string.Empty;
        }

        public abstract bool Exists();
    }
}
