using System;
using System.Data;
using Androtomist.Models.Forms;

namespace Androtomist.Models.Database.Inputs
{
    public abstract class TypeAbstract : DBClass
    {
        public string html { get; set; }
        public string name { get; set; }
        public string tableName { get; set; }
        public string column { get; set; }
        public string fitlerWith { get; set; }
		public Dependable d { get; set; }
		public string current { get; set; }
        public int min { get; set; }
        public int max { get; set; }
        public int step { get; set; }
		public bool is_parent { get; set; }

		public string placeholder { get; set; }

		public TypeAbstract(string tableName = "", string column = "", string name = "", string fitlerWith = "", string current = "", int min = 0, int max = 10, int step = 1, Dependable d = null, bool is_parent = false)
        {
            html = string.Empty;
        }

        public abstract void Populate(string SQL, string tableName);
    }
}
