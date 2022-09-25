using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Models
{
    public class InventoryMappings
    {
        [Key]
        public int idInventoryMappings {get; set;}
        public string invName { get; set; }
    }
}
