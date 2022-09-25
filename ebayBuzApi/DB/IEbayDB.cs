﻿using ebayBuzApi.Models;
using ebayBuzApi.Models.FormModels;
using ebayBuzApi.Models.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.DB
{
    public interface IEbayDB
    {
        public List<Inventory> GetAllCurrentInventory();
        public bool UpdateCurrentInventory(List<Inventory> inv);
        public bool DeleteInventory(List<string> idList);

        public List<Inventory> GetAllIncomingInventory();
        public double GetInventoryInvested();
        public bool AddIncomingInventory(Inventory inv);
        public bool MoveIncomingInventory(List<string> idList);
        public bool AddInventoryMapping(InventoryMappings item);
        public List<string> GetInventoryMappings();

        // Sales Controller
        public double GetTotalProfit();
        public List<MonthlySales> GetMonthlyProfit();
        public bool AddSalesRecord(SalesForm sale);
        public bool AddQuickSalesRecord(SalesForm sale);

    }
}
