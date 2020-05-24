using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Androtomist.Models.Forms
{
    public class FormResponse
    {
        public Int32 result { get; set; }
        public string msg { get; set; }
        public object data { get; set; }

        public FormResponse()
        {
            result = 1;
            msg = string.Empty;
            data = null;
        }

    }
}
