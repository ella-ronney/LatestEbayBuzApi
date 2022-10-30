using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Models.ResolutionCenter
{
    public class Returns
    {
        [Key]
        public int idReturns { get; set; }
        public string returnItemName { get; set; }
        public int returnedQty { get; set; }
        public float refundTotal { get; set; }
        public string returnTrackingNumber { get; set; }
        public string isVendorReturn { get; set; }

        // Ebay Returns
        public string? returnReason { get; set; }
        public string? warrantyClaim { get; set; }
        public string? sendBackToVendor { get; set; }

        // Vendor Returns
        public string? paymentMethod { get; set; }
        public string? returnVendor { get; set; }
        public DateTime? returnDate { get; set; }
        public DateTime? deliveryDate { get; set; }
    }
}
