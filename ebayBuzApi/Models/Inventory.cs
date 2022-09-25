using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Models
{
    public class Inventory
    {
        [Key]
        public int idInventory { get; set; }
        public string name { get; set; }
        public int qty { get; set; }
        public double unitPrice { get; set; }
        public string vendor { get; set; }
        public DateTime datePurchased { get; set; }
        public string payment { get; set; }
        public string? warranty { get; set; }
        public string currentInventory { get; set; }
        public double? salesTax { get; set; }

        public string condition { get; set; }

        // current inventory only 
        public DateTime? returnBy { get; set; }
        public string? ebayItemId { get; set; }

        // incoming inventory only 
        public DateTime? estimatedDelivery { get; set; }
        public string? trackingNumber { get; set; }
        public string dadPurchased { get; set; }
    }
}
