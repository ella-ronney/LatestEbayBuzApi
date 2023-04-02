using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Models.Expenses
{
    public class QuarterlyReporting
    {
        [Key]
        public string idQuarterlyReporting { get; set; }
        public string quarter { get; set; }
        public float quarterExpenses { get; set; }
        public float quarterProfit { get; set; }
        public float quarterInventoryCosts { get; set; }
        public float quarterFlSells { set; get; }
    }
}
