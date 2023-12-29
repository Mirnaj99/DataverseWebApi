using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Models.Cases
{
    public class GetCasesModel
    {
        public string title { get; set; }
        public string ticketnumber { get; set; }
        public string statuscode { get; set; }
        public string severitycode { get; set; }

        public string _customerid_value { get; set; }

    }
}
