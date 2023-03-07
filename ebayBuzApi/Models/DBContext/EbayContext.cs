using ebayBuzApi.Models.Expenses;
using ebayBuzApi.Models.ResolutionCenter;
using ebayBuzApi.Models.Sales;
using Microsoft.EntityFrameworkCore;

namespace ebayBuzApi.Models.DBContext
{
    public class EbayContext : DbContext
    {
        public EbayContext(DbContextOptions<EbayContext> options) : base(options) {}

        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<SaleRecords> SaleRecords { get; set; }
        public virtual DbSet<InventoryMappings> InventoryMappings { get; set; }
        public virtual DbSet<ArchievedSales> ArchievedSales { get; set; }
        public virtual DbSet<Fees> Fees { get; set; }
        public virtual DbSet<BusinessExpenses> BusinessExpenses { get; set; } 
        public virtual DbSet<Returns> Returns { get; set; }
        public virtual DbSet<eBaySaleRecord> eBaySaleRecord { get; set; }
        public virtual DbSet<CarLogRecords> CarLogRecords { get; set; }
    }
}
