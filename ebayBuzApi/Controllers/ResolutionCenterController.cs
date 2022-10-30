using ebayBuzApi.DB;
using ebayBuzApi.Models.ResolutionCenter;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResolutionCenterController : Controller
    {
        private IEbayDB ebayDBRecords;
        public ResolutionCenterController(IEbayDB ebayDBRecords)
        {
            this.ebayDBRecords = ebayDBRecords;
        }

        public bool AddReturn(Returns r)
        {
            return ebayDBRecords.AddReturn(r);
        }
    }
}
