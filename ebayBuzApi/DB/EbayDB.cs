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

namespace ebayBuzApi.DB
{
    public class EbayDB : IEbayDB
    {
        private EbayContext db;

        public EbayDB(EbayContext db)
        {
            this.db = db;
        }

        // Current Inventory Requests
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

        public bool UpdateCurrentInventory(List<Inventory> inv)
        {
            if (inv == null || inv.Count() < 1)
                return false;
            foreach (Inventory item in inv)
            {
                var currentItem = db.Inventory.Where(x => x.idInventory == item.idInventory).FirstOrDefault();
                if (currentItem == null)
                    return false;
                currentItem.ebayItemId = item.ebayItemId;
                db.Inventory.Update(currentItem);
                db.SaveChanges();
            }
            return true;
        }

        public bool DeleteInventory(List<string> idList)
        {
            if (!DBHelper.NullCheckForIdListIds(idList))
                return false;
            try
            {
                foreach (string id in idList)
                {
                    int itemId = 0;
                    if (DBHelper.ConvertStringToPosInt(id, ref itemId))
                    {
                        var item = db.Inventory.Where(x => x.idInventory == itemId).FirstOrDefault();
                        if (item == null)
                        {
                            return false;
                        }
                        db.Inventory.Remove(item);
                        db.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("failed to convert the id to int");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return true;
        }
        // Incoming Inventory Requests
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
                return db.Inventory.Select(x => x.unitPrice * x.qty).ToList().Sum();
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
                inv.unitPrice = Math.Round(inv.unitPrice / (double)inv.qty, 2);
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

        // Sales Controller

        public double GetTotalProfit()
        {
            return db.SaleRecords.Select(x => x.netProfit).Sum();
        }

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

        public List<MonthlySales> GetMonthlyProfit()
        {
            return db.SaleRecords.GroupBy(x => x.recordDate.Month, (key, group) => new MonthlySales
            {
                month = getMonthName(key),
                sum = group.Sum(y => y.netProfit)
            }).ToList();
        }

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

        // Business Controller

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

        // Resolution Center Controller
        public bool AddReturn(Returns r)
        {
            if (r == null)
                return false;

            db.Returns.Add(r);
            db.SaveChanges();
            return true;
        }

        // eBay Sales Excel Reader
        public bool AddEbaySaleRecord(eBaySaleRecord saleRecord)
        {
            if (saleRecord == null)
                return false;
            db.eBaySaleRecord.Add(saleRecord);
            db.SaveChanges();
            return true;
        }

        public List<eBaySaleRecord> GetAllEbaySaleRecords()
        {
            return db.eBaySaleRecord.ToList();
        }

        public bool UpdateEbaySaleRecords(List<eBaySaleRecord> records)
        {
            if (records == null)
                return false;

            foreach(eBaySaleRecord rec in records)
            {
                var currentRec = db.eBaySaleRecord.Where(x => x.idEbaySaleRecord == rec.idEbaySaleRecord).FirstOrDefault();
                if (currentRec == null)
                    return false;

                db.eBaySaleRecord.Update(currentRec);
                db.SaveChanges();
            }
            return true;
        }

        // Car Log Records
        public bool AddCarRecord(CarRecords carRecord)
        {
            if (carRecord == null)
                return false;
            if (carRecord.purpose == "Package DropOff")
                carRecord.distanceTraveled = BusinessExpenseHelper.GetDropOffLocationDistanceTraveled(carRecord.destination, carRecord.startDate, carRecord.endDate);
            db.CarLogRecords.Add(BusinessExpenseHelper.MapCarRecordsToCarLogRecords(carRecord));
            db.SaveChanges();
            return true;
        }

    }
}
