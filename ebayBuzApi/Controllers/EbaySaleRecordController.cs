using ebayBuzApi.DB;
using ebayBuzApi.Helpers;
using ebayBuzApi.Models;
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
    public class EbaySaleRecordController : Controller
    {
        private IEbayDB ebayDBRecords;
        private eBaySalesExcelReader ebayExcelReader;

        public EbaySaleRecordController(IEbayDB ebayDBRecords)
        {
            this.ebayDBRecords = ebayDBRecords;
        }

        [HttpPost]
        [Route("AddEbayExcelRecords")]
        public bool AddEbayExcelRecods(eBayExcelFile file)
        {
            ebayExcelReader = new eBaySalesExcelReader(ebayDBRecords, file.filePath, file.startDate, file.endDate);
            return ebayExcelReader.ReadEbayExcelWorkbook();
        }

        [HttpGet]
        [Route("eBayExcelSaleRecords")]
        public ActionResult<List<eBaySaleRecord>> GetAllEbaySaleRecords()
        {
            return ebayDBRecords.GetAllEbaySaleRecords();
        }

        [HttpPut]
        [Route("UpdateEbaySaleRecord")]
        public bool UpdateEbaySaleRecords(List<eBaySaleRecord> records)
        {
            return ebayDBRecords.UpdateEbaySaleRecord(records);
        }

        [HttpGet]
        [Route("TotalProfit")]
        public double TotalProfit()
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
        [Route("AddNonWASale")]
        public bool AddNonWaSale(NonWASale record)
        {
            return ebayDBRecords.AddNonWASale(record);
        }

        [HttpGet]
        [Route("NonWASells")]
        public List<NonWASale> GetNonWASells()
        {
            return ebayDBRecords.GetNonWASells();
        }

        [HttpPut]
        [Route("UpdateNonWASells")]
        public bool UpdateNonWASells(List<NonWASale> sales)
        {
            return ebayDBRecords.UpdateNonWASells(sales);
        }
    }
}
