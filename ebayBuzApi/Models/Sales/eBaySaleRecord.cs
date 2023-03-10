using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Models.Sales
{
    public class eBaySaleRecord
    {
        [Key]
        public int idEbaySaleRecord { get; set; }
        public string listingTitle { get; set; }
        public string ebayItemId { get; set; }
        public int quantitySold { get; set; } 
        public double totalSales { get; set; }
        public double totalProfit { get; set; }
        public double totalSellingCosts { get; set; }
        public double avgSellingPrice { get; set; }
        public double profitPercentage { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
