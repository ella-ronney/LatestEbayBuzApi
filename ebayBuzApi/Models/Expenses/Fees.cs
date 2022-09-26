using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Models.Expenses
{
    public class Fees
    {
        [Key]
        public int idFees { get; set; }
        public string feeType { get; set; }
        public double amount { get; set; }
        public string note { get; set; }
    }
}
