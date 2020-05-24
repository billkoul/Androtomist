using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Androtomist.Models.Database.Datatables
{
    public class DataTableResult
    {

        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public int sEcho { get; set; }
        public string sColumns { get; set; }
        public List<dynamic> aaData { get; set; }

        public DataTableResult()
        {
            iTotalRecords = 350;
            iTotalDisplayRecords = 350;
            sEcho = 0;
            sColumns = string.Empty;
            aaData = new[] {
                new { name = "toto", age = 5 },
                new { name = "titi", age = 7 },
                new { name = "tata", age = 3 },
                new { name = "tutu", age = 4 }
            }.Select(x =>
            {
                dynamic d = new ExpandoObject();
                d.RecordID = 2;
                d.OrderID = "63629-4697";
                d.Country = "Indonesia";
                d.ShipCity = "Cihaur";
                d.CompanyAgent = "Emelita Giraldez";
                d.ShipDate = "8/6/2017";
                d.Status = 6;
                d.Type = 3;
                d.Actions = null;
                return d;
            }).ToList();
        }
    }



}
