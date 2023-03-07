using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Models
{
    public class CarLogRecords
    {
        [Key]
        public int idCarLogRecords { get; set; }
        public string car { get; set; }
        public string purpose { get; set; }
        public string destination { get; set; }
        public float distanceTraveled { get; set; }
        public DateTime startDate { get; set; }
        public DateTime? endDate { get; set; }
    }
}
