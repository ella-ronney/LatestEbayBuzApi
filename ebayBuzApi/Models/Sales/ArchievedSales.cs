using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Models
{
    public class ArchievedSales
    {
        [Key]
        public int idArchievedSales { get; set; }
        public string itemName { get; set; }
        public double unitCost { get; set; }
        public string vendor { get; set; }
        public DateTime datePurchased { get; set; }
        public int inventoryIdentifier { get; set; }
        public int qty { get; set; }
    }
}
