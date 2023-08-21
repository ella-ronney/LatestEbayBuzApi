using ebayBuzApi.Models;
using ebayBuzApi.Models.Car;
using ebayBuzApi.Models.Expenses;
using ebayBuzApi.Models.FormModels;
using ebayBuzApi.Models.ResolutionCenter;
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
        public bool UpdateInventory(List<Inventory> inv);
        public bool DeleteInventory(IdList idList);

        public List<Inventory> GetAllIncomingInventory();
        public double GetInventoryInvested();
        public bool AddIncomingInventory(Inventory inv);
        public bool MoveIncomingInventory(List<string> idList);
        public bool AddInventoryMapping(InventoryMappings item);
        public List<string> GetInventoryMappings();
        public bool AddSeparateComponentsRecord(SeparateItemComponentsRecord record);
        public bool AddCombinedSaleRecord(CombinedSaleRecord record);

        // Sales Controller
        public double GetTotalProfit();
        public List<MonthlySales> GetMonthlyProfit();
        public bool AddSalesRecord(SalesForm sale);
        public bool AddQuickSalesRecord(SalesForm sale);
        public bool AddNonWASale(NonWASale record);
        public List<NonWASale> GetNonWASells();
        public bool UpdateNonWASells(List<NonWASale> sales);

        // Business Controller
        public List<ExpenseTotals> GetAllExpensesTotals();
        public bool AddExpense(BusinessExpenses expense);

        // Resolution Center Controller
        public bool AddReturn(Returns r);
        public List<Returns> GetAllVendorReturns();
        public bool UpdateVendorReturn(List<Returns> returns);
        public List<Returns> GetAllEbayReturns();
        public bool DeleteReturn(IdList idList);

        // eBay Sales Excel Reader
        public bool AddEbaySaleRecord(eBaySaleRecord saleRecord);
        public List<eBaySaleRecord> GetAllEbaySaleRecords();
        public bool UpdateEbaySaleRecord(List<eBaySaleRecord> records);
        public bool AddCarRecord(CarRecords carRecord);
        public List<YearlyCarLogs> GetCarLogs();
        public bool UpdateCarLogTotalMiles(YearlyCarLogs log);
        public bool GetQuarterlyReporting(QuarterReportingForm qForm);

    }
}
