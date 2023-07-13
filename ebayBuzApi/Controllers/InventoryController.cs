using ebayBuzApi.DB;
using ebayBuzApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : Controller
    {
        private IEbayDB ebayDBRecords;
        public InventoryController(IEbayDB ebayDBRecords)
        {
            this.ebayDBRecords = ebayDBRecords;
        }
        // Current Inventory Requests
        [HttpGet]
        [Route("CurrentInventory")]
        public ActionResult<List<Inventory>> GetAllCurrentInventory()
        {
            return ebayDBRecords.GetAllCurrentInventory();
        }

        [HttpPut]
        [Route("UpdateInventory")]
        public bool UpdateInventory(List<Inventory> inv)
        {
            return ebayDBRecords.UpdateInventory(inv);
        }

        [HttpDelete]
        [Route("DeleteInventory")]
        public bool DeleteInventory(IdList IdList)
        {
            if (ebayDBRecords.DeleteInventory(IdList))
            {
                return true;
            }
            return false;
        }


        // Incoming Inventory Requests
        [HttpGet]
        [Route("IncomingInventory")]
        public ActionResult<List<Inventory>> GetAllIncomingInventory()
        {
            return ebayDBRecords.GetAllIncomingInventory();
        }

        [HttpGet]
        [Route("InventoryInvested")]
        public ActionResult<double> GetInventoryInvested()
        {
            return ebayDBRecords.GetInventoryInvested();
        }

        [HttpPost]
        [Route("AddIncomingInventory")]
        public ActionResult<Inventory> AddIncomingInventory(Inventory inv)
        {
            if (ebayDBRecords.AddIncomingInventory(inv))
            {
                return inv;
            }
            return null;
        }

        [HttpPut]
        [Route("MoveIncomingInventory")]
        public bool MoveIncomingInventory(List<string> idList)
        {
            return ebayDBRecords.MoveIncomingInventory(idList);
        }

        // Inventory Mapping
        [HttpPost]
        [Route("AddInventoryMapping")]
        public bool AddInventoryMapping(InventoryMappings item)
        {
            return ebayDBRecords.AddInventoryMapping(item);
        }

        [HttpGet]
        [Route("InventoryMappings")]
        public ActionResult<List<string>> GetInventoryMappings()
        {
            return ebayDBRecords.GetInventoryMappings();
        }

        [HttpPost]
        [Route("AddSeparateComponentsRecord")]
        public bool AddSeperateComponentsRecord(SeparateItemComponentsRecord record)
        {
            return ebayDBRecords.AddSeparateComponentsRecord(record);
        }

        [HttpPost]
        [Route("AddCombinedSaleRecord")]
        public bool AddCombinedSaleRecord(CombinedSaleRecord record)
        {
            return ebayDBRecords.AddCombinedSaleRecord(record);
        }
    }
}
