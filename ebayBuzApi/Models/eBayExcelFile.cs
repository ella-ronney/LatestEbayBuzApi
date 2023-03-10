using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Models
{
    public class eBayExcelFile
    {
        public string filePath {get; set;}
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
