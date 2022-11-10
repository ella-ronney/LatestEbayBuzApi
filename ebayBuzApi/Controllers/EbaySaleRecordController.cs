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
        public bool AddEbayExcelRecods(EbayExcelFilePath filePath)
        {
            ebayExcelReader = new eBaySalesExcelReader(ebayDBRecords, filePath.name);
            return ebayExcelReader.ReadEbayExcelWorkbook();
        }

        [HttpGet]
        [Route("eBayExcelSaleRecords")]
        public ActionResult<List<eBaySaleRecord>> GetAllEbaySaleRecords()
        {
            return ebayDBRecords.GetAllEbaySaleRecords();
        }

        [HttpPut]
        [Route("UpdateEbaySaleRecords")]
        public bool UpdateEbaySaleRecords(List<eBaySaleRecord> records)
        {
            return ebayDBRecords.UpdateEbaySaleRecords(records);
        }

    }
}
