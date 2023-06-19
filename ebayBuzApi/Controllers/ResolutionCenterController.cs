using ebayBuzApi.DB;
using ebayBuzApi.Models;
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

        [HttpPost]
        [Route("AddReturn")]
        public bool AddReturn(Returns r)
        {
            return ebayDBRecords.AddReturn(r);
        }

        [HttpGet]
        [Route("VendorReturns")]
        public List<Returns> GetAllVendorReturns()
        {
            return ebayDBRecords.GetAllVendorReturns();
        }

        [HttpGet]
        [Route("eBayReturns")]
        public List<Returns> GetAllEbayReturns()
        {
            return ebayDBRecords.GetAllEbayReturns();
        }

        [HttpDelete]
        [Route("DeleteReturn")]
        public bool DeleteReturn(IdList idList)
        {
            return ebayDBRecords.DeleteReturn(idList);
        }
    }
}
