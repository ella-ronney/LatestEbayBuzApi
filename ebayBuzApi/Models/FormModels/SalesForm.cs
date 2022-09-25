using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Models.FormModels
{
    public class SalesForm
    {
        public string saleType { get; set; }
        public string? ebayId { get; set; }
        public string? itemName { get; set; }
        public int qtySold { get; set; }
        public DateTime recordDate { get; set; }
        public double? shippingCost { get; set; }
        public double? ebayFees { get; set; }
        public double? promoFees { get; set; }
        public double? totalCost { get; set; }
        public double totalPriceSold { get; set; }
        public double? salesTax { get; set; }
    }
}
