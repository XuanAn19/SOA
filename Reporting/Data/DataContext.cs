using Microsoft.EntityFrameworkCore;
using Reporting.Models;

namespace Reporting.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<OrderReportModel> orders_reports {  get; set; }  
        public DbSet<ProductReportModel> product_reports {  get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
