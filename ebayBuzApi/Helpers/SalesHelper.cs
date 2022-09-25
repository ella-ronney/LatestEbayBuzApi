using ebayBuzApi.Models;
using ebayBuzApi.Models.FormModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Helpers
{
    public static class SalesHelper
    {
        public static SaleRecords MapSalesFormToSalesRecord(SalesForm sale, Inventory inv)
        {
            // FIXME - Handle facebook sales logic
            SaleRecords saleRecord =  new SaleRecords
            {
                itemName = sale.itemName,
                qtySold = sale.qtySold,
                avgSellingPrice = sale.totalPriceSold / (double)sale.qtySold,
                saleType = sale.saleType,
                recordDate = sale.recordDate,
            };

            double ebayCosts = (double)(sale.shippingCost + sale.ebayFees + sale.promoFees); 

            if (sale.saleType == "Adorama")
            {
                saleRecord.netProfit = sale.totalPriceSold - ((double)sale.totalCost + ebayCosts);
                saleRecord.profitPercentage = (saleRecord.netProfit / sale.totalPriceSold * 100);
            }
            else
            {
                if(inv!= null)
                {
                    saleRecord.netProfit = sale.totalPriceSold - (inv.unitPrice * sale.qtySold + ebayCosts);
                    saleRecord.profitPercentage = saleRecord.netProfit / sale.totalPriceSold * 100;
                }
                else
                {
                    return null;
                }
                
            }
            return saleRecord;
        }

        public static ArchievedSales MapSaleToArchieveRecord(SalesForm sale, Inventory inv)
        {
            ArchievedSales archievedSale = new ArchievedSales
            {
                unitCost = inv.unitPrice,
                vendor = inv.vendor,
                datePurchased = inv.datePurchased,
                inventoryIdentifier = inv.idInventory,
                qty = sale.qtySold,
            };

            if (sale.saleType == "eBay")
            {
                archievedSale.itemName = inv.name;
            }
            else
            {
                archievedSale.itemName = sale.itemName;
            }

            return archievedSale;
        }
    }
}
