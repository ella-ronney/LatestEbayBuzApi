using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Models.Sales
{
    public class NonWASale
    {
        [Key]
        public int idNonWASale { get; set; }
        public string ebayItemId { get; set; }
        public string name { get; set; }
        public int qty { get; set; }
        public string quarter { get; set; }
        public string year { get; set; }
    }
}
