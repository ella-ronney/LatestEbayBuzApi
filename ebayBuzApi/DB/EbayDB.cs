using ebayBuzApi.Helpers;
using ebayBuzApi.Models;
using ebayBuzApi.Models.Expenses;
using ebayBuzApi.Models.DBContext;
using ebayBuzApi.Models.FormModels;
using ebayBuzApi.Models.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ebayBuzApi.Models.ResolutionCenter;
using ebayBuzApi.Models.Car;

namespace ebayBuzApi.DB
{
    public class EbayDB : IEbayDB
    {
        private EbayContext db;

        public EbayDB(EbayContext db)
        {
            this.db = db;
        }

        int currentQuarter = ((DateTime.Now.Month + 2) / 3) -1 ;
        int currentYear = DateTime.Now.Year;
        #region Current Inventory
        public List<Inventory> GetAllCurrentInventory()
        {
            try
            {
                // The database is empty
                if (db.Inventory == null)
                {
                    return null;
                }
                return db.Inventory.Where(x => x.currentInventory == "true").ToList();
            }
            catch (Exception ex)
            {
                // FIXME log the error
                return null;
            }
        }

        public bool UpdateInventory(List<Inventory> inv)
        {
            if (inv == null || inv.Count() < 1)
                return false;

            foreach (Inventory item in inv)
            {
                var currentItem = db.Inventory.Where(x => x.idInventory == item.idInventory).FirstOrDefault();
                
                if (currentItem == null)
                    return false;

                if(currentItem.ebayItemId != item.ebayItemId)
                    currentItem.ebayItemId = item.ebayItemId;

                if (currentItem.qty != item.qty)
                    currentItem.qty = item.qty;

                if (currentItem.currentInventory != item.currentInventory)
                    currentItem.currentInventory = item.currentInventory;

                db.Inventory.Update(currentItem);
                db.SaveChanges();
            }
            return true;
        }

        public bool DeleteInventory(IdList idList)
        {
            if (idList == null || String.IsNullOrEmpty(idList.id))
                return false;
            try
            {
                int id = Int32.Parse(idList.id);
                var item = db.Inventory.Where(x => x.idInventory == id).FirstOrDefault();
                if (item == null)
                    return false;
                db.Remove(item);
                db.SaveChanges();     
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
            return true;
        }
        #endregion

        #region Incoming Inventory
        public List<Inventory> GetAllIncomingInventory()
        {
            try
            {
                // The database is empty
                if (db.Inventory == null)
                {
                    return null;
                }
                return db.Inventory.Where(x => x.currentInventory == "false").ToList();
            }
            catch (Exception ex)
            {
                // FIXME log the error
                return null;
            }
        }

        public double GetInventoryInvested()
        {
            try
            {
                // The database is empty
                if (db.Inventory == null)
                {
                    return 0;
                }
                return Math.Round(db.Inventory.Select(x => x.unitPrice * x.qty).ToList().Sum(),2);
            }
            catch (Exception ex)
            {
                // FIXME log the error
                return 0;
            }
        }

        public bool AddIncomingInventory(Inventory inv)
        {
            // FIXME add a validator for all of the inv attributes - before adding to the db... do for all add statements
            if (inv != null)
            {
                inv.unitPrice = Math.Round(inv.unitPrice, 2);
                db.Inventory.Add(inv);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool MoveIncomingInventory(List<string> idList)
        {
            if (!DBHelper.NullCheckForIdListIds(idList))
                return false;
            foreach (string id in idList)
            {
                int itemId = 0;
                if (DBHelper.ConvertStringToPosInt(id, ref itemId))
                {
                    var item = db.Inventory.Where(x => x.idInventory == itemId).FirstOrDefault();
                    if (item == null)
                        return false;
                    item.currentInventory = "true";
                    db.Inventory.Update(item);
                    db.SaveChanges();
                }
                else
                {
                    Console.WriteLine("failed to convert the id to int");
                    return false;
                }
            }
            return true;
        }

        public bool AddInventoryMapping(InventoryMappings item)
        {
            if (item.invName != null && item.invName.Length > 0)
            {
                db.InventoryMappings.Add(item);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public List<string> GetInventoryMappings()
        {
            return db.InventoryMappings.Select(x=>x.invName).ToList();
        }
        #endregion

        #region Sales Controller - Depreciate?

        /*public double GetTotalProfit()
        {
            return db.SaleRecords.Select(x => x.netProfit).Sum();
        }*/

        public bool AddSalesRecord(SalesForm sale)
        {
            Inventory inv;
            if (sale.saleType == "eBay")
            {
                inv = db.Inventory.Where(x => x.ebayItemId == sale.ebayId).FirstOrDefault();

                // Only remove from inventory db if the qty is 1 otherwise reduce quantity by 1
                if (inv.qty == 1)
                    db.Inventory.Remove(inv);
                else
                {
                    inv.qty = inv.qty - 1;
                    db.Inventory.Update(inv);
                } 
               
                db.SaveChanges();
            }
            else
                inv = null;

            if (SalesHelper.MapSalesFormToSalesRecord(sale, inv) == null)
                return false;

            addEbayFeesToDB(sale);
            db.SaleRecords.Add(SalesHelper.MapSalesFormToSalesRecord(sale, inv));
            db.ArchievedSales.Add(SalesHelper.MapSaleToArchieveRecord(sale, inv));
            db.SaveChanges();
            return true;
        }

        public bool AddQuickSalesRecord(SalesForm sale)
        {
            double ebayFees = (double)(sale.shippingCost + sale.ebayFees + sale.promoFees);
            addEbayFeesToDB(sale);

            SaleRecords s = new SaleRecords
            {
                itemName = sale.itemName,
                qtySold = sale.qtySold,
                avgSellingPrice = sale.totalPriceSold / (double)sale.qtySold,
                netProfit = sale.totalPriceSold - ((double)sale.totalCost + ebayFees),
                saleType = sale.saleType,
                recordDate = sale.recordDate,
            };
            s.profitPercentage = s.netProfit / sale.totalPriceSold * 100;
            db.SaleRecords.Add(s);
            db.SaveChanges();
            return true;
        }

        public bool AddNonWASale (NonWASale record)
        {
            if (record == null)
                return false;
            if (String.IsNullOrEmpty(record.year))
                record.year = currentYear.ToString();

            db.NonWASale.Add(record);
            db.SaveChanges();
            return true;
        }

        public List<NonWASale> GetNonWASells()
        {
            return db.NonWASale.ToList();
        }

        public bool UpdateNonWASells(List<NonWASale> sales)
        {
            if (sales.Count == 0 || sales == null)
                return false;

            foreach (NonWASale rec in sales)
            {
                var currentRec = db.NonWASale.Where(x => x.idNonWASale == rec.idNonWASale).FirstOrDefault();
                if (currentRec == null)
                    return false;
                currentRec.qty = rec.qty;
                db.NonWASale.Update(currentRec);
                db.SaveChanges();
            }
            return true;
        }

        /*public List<MonthlySales> GetMonthlyProfit()
        {
            return db.SaleRecords.GroupBy(x => x.recordDate.Month, (key, group) => new MonthlySales
            {
                month = getMonthName(key),
                sum = group.Sum(y => y.netProfit)
            }).ToList();
        }*/

        private void addEbayFeesToDB(SalesForm sale)
        {
            var ship = db.Fees.Where(x => x.feeType == "shippingCosts").FirstOrDefault();
            var ebayFee = db.Fees.Where(x => x.feeType == "ebayFees").FirstOrDefault();
            var ebayPromoFee = db.Fees.Where(x => x.feeType == "ebayPromoFees").FirstOrDefault();
            ship.amount = ship.amount + (double)sale.shippingCost;
            ebayFee.amount = ebayFee.amount + (double)sale.ebayFees;
            ebayPromoFee.amount = ebayPromoFee.amount + (double)sale.promoFees;
            db.Update(ship);
            db.Update(ebayFee);
            db.Update(ebayPromoFee);
            db.SaveChanges();
        }

        private static string getMonthName(int month)
        {
            DateTime date = new DateTime(2020, month, 1);
            return date.ToString("MMM");
        }
        #endregion

        #region Business Controller

        public List<ExpenseTotals> GetAllExpensesTotals()
        {
            if (db.BusinessExpenses == null)
                return null;
            List<BusinessExpenses> allExpenseRecords = db.BusinessExpenses.Where(MySqlX => MySqlX.purchaseDate.Year == DateTime.Now.Year).ToList();
            // TODO - calculate inventory costs current year
            return BusinessExpenseHelper.SetExpenseTotals(allExpenseRecords);
        }

        public bool AddExpense(BusinessExpenses expense)
        {
            if (expense == null)
                return false;

            db.BusinessExpenses.Add(expense);
            db.SaveChanges();
            return true; 
        }

        public bool GetQuarterlyReporting(QuarterReportingForm qForm)
        {
            if (!String.IsNullOrEmpty(qForm.quarter))
            {
                int formQuarter = 0;
                Int32.TryParse(qForm.quarter.Substring(1,1), out formQuarter);
                currentQuarter = currentQuarter == formQuarter ? currentQuarter : formQuarter;
            }

            if (!String.IsNullOrEmpty(qForm.year))
            {
                currentYear = currentYear.ToString() == qForm.year ? currentYear : Int32.Parse(qForm.year);
            }
            
            var quaterProfit = GetQuarterlyProfit();
            var quarterExpenses = db.BusinessExpenses.Where(x => x.purchaseDate.Year == currentYear && x.expenseCategory != "Inventory" &&
            ((x.purchaseDate.Month + 2) / 3) >= currentQuarter && ((x.purchaseDate.Month + 2) / 3) < currentQuarter + 1).Select(x=>x.cost * (x.businessPercentage/100)).Sum();

            var quarterWAReporting = quaterProfit - (quarterExpenses + qForm.flSells);
            return true;
        }

        #endregion

        #region Resolution Center Controller
        public bool AddReturn(Returns r)
        {
            if (r == null)
                return false;

            db.Returns.Add(r);
            db.SaveChanges();
            return true;
        }

        public List<Returns> GetAllVendorReturns()
        {
            return db.Returns.Where(x => x.isVendorReturn == "Yes").ToList();
        }

        public List<Returns> GetAllEbayReturns()
        {
            return db.Returns.Where(x => x.isVendorReturn == "No").ToList();
        }

        public bool DeleteReturn(IdList idList)
        {
            if (idList == null || String.IsNullOrEmpty(idList.id))
                return false;

            try
            {
                int id = Int32.Parse(idList.id);
                var item = db.Returns.Where(x => x.idReturns == id).FirstOrDefault();
                if (item == null)
                    return false;
                db.Remove(item);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
            return true;
        }

        #endregion

        #region eBay Sales Excel Reader
        public double GetTotalProfit()
        {
            return Math.Round(db.eBaySaleRecord.Select(x => x.totalProfit).Sum(),2);
        }

        public List<MonthlySales> GetMonthlyProfit()
        {
            return db.eBaySaleRecord.GroupBy(x => x.startDate.Month, (key, group) => new MonthlySales
            {
                month = getMonthName(key),
                sum = Math.Round(group.Sum(y => y.totalProfit),2)
            }).ToList();
        }

        public double GetQuarterlyProfit()
        {
            
            return db.eBaySaleRecord.Where(x => x.startDate.Year == currentYear && ((x.startDate.Month+2)/3) >= currentQuarter && ((x.startDate.Month + 2) / 3) < currentQuarter + 1)
                .Select(x => x.totalProfit).Sum();
            
        }

        public bool AddEbaySaleRecord(eBaySaleRecord saleRecord)
        {
            if (saleRecord == null)
                return false;
            
            double totalInvCosts = UpdateInventoryRecordOnSale(saleRecord.ebayItemId, saleRecord.quantitySold);
            CalculateProfitValuesOnSale(ref saleRecord, totalInvCosts);
            db.eBaySaleRecord.Add(saleRecord);
            db.SaveChanges();
            return true;
        }

        public List<eBaySaleRecord> GetAllEbaySaleRecords()
        {
            return db.eBaySaleRecord.ToList();
        }

        public bool UpdateEbaySaleRecord(List<eBaySaleRecord> record)
        {
            if (record == null || record.Count()==0)
                return false;

            foreach(eBaySaleRecord rec in record)
            {
                var currentRec = db.eBaySaleRecord.Where(x => x.idEbaySaleRecord == rec.idEbaySaleRecord).FirstOrDefault();
                if (currentRec == null)
                    return false;
                double additionalCosts = rec.totalSellingCosts - currentRec.totalSellingCosts;
                currentRec.totalProfit -= additionalCosts;
                currentRec.totalSellingCosts = rec.totalSellingCosts;
                db.eBaySaleRecord.Update(currentRec);
                db.SaveChanges();
            }
            return true;
        }

        private double UpdateInventoryRecordOnSale(string eBayItemId, int qtySold)
        {
            List<Inventory> inv = db.Inventory.Where(x => x.ebayItemId == eBayItemId).ToList();
            if (inv.Count() == 0)
                return 0;
            
            int listIndex = 0;
            double totalInvCosts = 0;
            while (listIndex < inv.Count() && qtySold > 0)
            {
                if (inv[listIndex].qty <= qtySold)
                {
                    // TODO add archieved sale logic here to preserve unit cost over time
                    db.Inventory.Remove(inv[listIndex]);
                    totalInvCosts += inv[listIndex].qty * inv[listIndex].unitPrice;
                    qtySold = qtySold - inv[listIndex].qty;
                    listIndex++;
                }
                else
                {
                    inv[listIndex].qty = inv[listIndex].qty - qtySold;
                    totalInvCosts += qtySold * inv[listIndex].unitPrice;
                    db.Inventory.Update(inv[listIndex]);
                    qtySold = 0;
                }
            }
            db.SaveChanges();
            return Math.Round(totalInvCosts,2);
        }

        private void CalculateProfitValuesOnSale(ref eBaySaleRecord record, double totalInvCosts)
        {
            record.totalSellingCosts += totalInvCosts;
            record.totalProfit = Math.Round(record.totalSales - record.totalSellingCosts,2);
            if(totalInvCosts != 0)
                record.profitPercentage = Math.Round((record.totalProfit / totalInvCosts) * 100,2);
        }
        #endregion

        #region Car Log Records
        public bool AddCarRecord(CarRecords carRecord)
        {
            if (carRecord == null)
                return false;
            if (carRecord.purpose == "Package DropOff")
                carRecord.distanceTraveled = BusinessExpenseHelper.GetDropOffLocationDistanceTraveled(carRecord.destination, carRecord.startDate, carRecord.endDate, carRecord.satDropOff);

            AddYearlyCarLog(carRecord);
            db.CarLogRecords.Add(BusinessExpenseHelper.MapCarRecordsToCarLogRecords(carRecord));
            db.SaveChanges();
            return true;
        }

        public List<YearlyCarLogs> GetCarLogs()
        {
            return db.YearlyCarLogs.ToList();
        }

        // TODO maybe send back the percentage so it can be updated on put call
        public bool UpdateCarLogTotalMiles(YearlyCarLogs log)
        {
            if (log == null)
                return false;

            YearlyCarLogs carLog = db.YearlyCarLogs.Where(x => x.car == log.car && x.year == log.year).FirstOrDefault();
            carLog.totalMiles = log.totalMiles;
            carLog.businessUsagePercentage = (carLog.businessMiles / (carLog.totalMiles-carLog.totalMilesStartYear)) * 100f;
            db.YearlyCarLogs.Update(carLog);
            db.SaveChanges();
            return true;
        }

        private void AddYearlyCarLog(CarRecords record)
        {
            YearlyCarLogs yearlyLogs =  db.YearlyCarLogs.Where(x => x.car == record.car && x.year == record.startDate.Year.ToString()).FirstOrDefault();
            yearlyLogs.businessMiles += record.distanceTraveled;
            db.YearlyCarLogs.Update(yearlyLogs);
            db.SaveChanges();
        }
        #endregion

    }
}
