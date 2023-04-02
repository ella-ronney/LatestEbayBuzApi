using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Models.Expenses
{
    public class QuarterReportingForm
    {
        public float flSells { get; set; }
        public string quarter { get; set; }
        public string year { get; set; }
    }
}
