using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Models
{
    public class SaleRecords
    {
        [Key]
        public int idSaleRecords { get; set; }
        public string itemName { get; set; }
        public double profitPercentage { get; set; }
        public int qtySold { get; set; }
        public double avgSellingPrice { get; set; }
        public double netProfit { get; set; }
        public string saleType { get; set; }
        public DateTime recordDate { get; set; }
    }
}
