using ebayBuzApi.DB;
using ebayBuzApi.Models;
using ebayBuzApi.Models.FormModels;
using ebayBuzApi.Models.Sales;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : Controller
    {
        private IEbayDB ebayDBRecords;
        public SalesController(IEbayDB ebayDBRecords)
        {
            this.ebayDBRecords = ebayDBRecords;
        }
        [HttpGet]
        [Route("TotalProfit")]
        public ActionResult<double> GetTotalProfit()
        {
            return ebayDBRecords.GetTotalProfit();
        }

        [HttpGet]
        [Route("MonthlyProfit")]
        public ActionResult<List<MonthlySales>> GetMonthlyProfit()
        {
            return ebayDBRecords.GetMonthlyProfit();
        }

        [HttpPost]
        [Route("AddSalesRecord")]
        public ActionResult<bool> AddSalesRecord(SalesForm sale)
        {
            return ebayDBRecords.AddSalesRecord(sale);
        }

        [HttpPost]
        [Route("AddQuickSalesRecord")]
        public bool AddQuickSalesRecord(SalesForm sale)
        {
            return ebayDBRecords.AddQuickSalesRecord(sale);
        }

      
    }
}
