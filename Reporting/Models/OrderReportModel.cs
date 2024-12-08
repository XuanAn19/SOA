using System.ComponentModel.DataAnnotations;

namespace Reporting.Models
{
    public class OrderReportModel
    {
        [Key]
        public int id { get; set; }
        public int order_id { get; set; }
        public decimal total_revenue { get; set; }
        public decimal total_cost { get; set; }
        public decimal total_profit { get; set; }
        public ICollection<ProductReportModel> ProductReports { get; set; }
    }
}
