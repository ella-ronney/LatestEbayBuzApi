using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Models.Car
{
    public class YearlyCarLogs
    {
        [Key]
        public int idyearlycarlogs { get; set; }
        public string car { get; set; }
        public float businessMiles { get; set; }
        public float totalMiles { get; set; }
        public float businessUsagePercentage { get; set; }
        public float totalMilesStartYear { get; set; }
        public string year { get; set; }
    }
}
