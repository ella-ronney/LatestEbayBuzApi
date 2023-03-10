using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ebayBuzApi.DB;
using ebayBuzApi.Models.Sales;
using IronXL;

namespace ebayBuzApi.Helpers
{
    public class eBaySalesExcelReader
    {
        WorkBook workbook;
        WorkSheet sheet;
        private IEbayDB ebayDBRecords;
        private DateTime startDate;
        private DateTime endDate;

        public eBaySalesExcelReader(IEbayDB ebayDBRecords, string fileName, DateTime startDate, DateTime endDate)
        {
            workbook = WorkBook.Load("C:\\Users\\miran\\OneDrive\\Desktop\\" + fileName +".xlsx");
            sheet = workbook.WorkSheets.First();
            this.startDate = startDate;
            this.endDate = endDate;
            this.ebayDBRecords = ebayDBRecords;
        }

        public bool ReadEbayExcelWorkbook()
        {
            try
            {
                DataTable dt = sheet.ToDataTable(true);
                foreach (var row in dt.AsEnumerable())
                {
                    var listingTitle = row.Field<string>("Listing title");
                    var eBayItemId = row.Field<double>("eBay item ID");
                    var quantitySold = row.Field<double>("Quantity sold");
                    var itemSales = row.Field<double>("Item sales");
                    var totalSellingCosts = row.Field<double>("Total selling costs");
                    var avgSellingPrice = row.Field<double>("Average Selling price");

                    addEbaySaleRecord(listingTitle, eBayItemId, quantitySold, itemSales, totalSellingCosts, avgSellingPrice);
                }
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false; 
            }
        }

        private void addEbaySaleRecord(string listingTitle, double eBayItemId, double quantitySold, double itemSales, double totalSellingCosts, double avgSellingPrice )
        {
            eBaySaleRecord record = new eBaySaleRecord
            {
                listingTitle = listingTitle,
                ebayItemId = eBayItemId.ToString(),
                quantitySold = (int)quantitySold,
                totalSales = itemSales,
                totalSellingCosts = totalSellingCosts,
                avgSellingPrice = avgSellingPrice,
                startDate = this.startDate,
                endDate = this.endDate,
            };

            ebayDBRecords.AddEbaySaleRecord(record);
        }
    }
}
