using System;
using System.Data;
using System.Collections.Generic;
using Androtomist.Models.Database;
using Androtomist.Models.Database.Entities;

namespace Androtomist.Models.Results
{
    public class APK
    {
        public string package { get; set; }
        public List<string> permissions { get; set; }
        public List<string> intent { get; set; }
    }
}
