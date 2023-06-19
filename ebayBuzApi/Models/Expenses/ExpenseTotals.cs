using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Models.Expenses
{
    public class ExpenseTotals
    {
        public float inventory { get; set; }
        public float supplies { get; set; }
        public float subscriptions { get; set; }
        public float officeSpace { get; set; }
        public float officeFurniture { get; set; }
        public float food { get; set; }
        public float car { get; set; }
        public float travel { get; set; }
        public float miscExpenses { get; set; }
        public float shippingCosts { get; set; }
        public float taxes { get; set; }
        public float total { get; set; }
    }
}
