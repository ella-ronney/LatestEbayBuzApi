using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Models
{
    public class SeparateItemComponentsRecord
    {
        public string itemId { get; set; }
        public int unitsToSeparate { get; set; }
        public int numberComponents { get; set; }
        public string? nameComponent1 { get; set; }
        public int? qtyComponent1 { get; set; }
        public double? priceComponent1 { get; set; }
        public string? nameComponent2 { get; set; }
        public int? qtyComponent2 { get; set; }
        public double? priceComponent2 { get; set; }
        public string? nameComponent3 { get; set; }
        public int? qtyComponent3 { get; set; }
        public double? priceComponent3 { get; set; }
        public string? nameComponent4 { get; set; }
        public int? qtyComponent4 { get; set; }
        public double? priceComponent4 { get; set; }
        public string? nameComponent5 { get; set; }
        public int? qtyComponent5 { get; set; }
        public double? priceComponent5 { get; set; }
    }
}
