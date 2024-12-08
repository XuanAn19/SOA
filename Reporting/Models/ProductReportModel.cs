using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Reporting.Models
{
    public class ProductReportModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("OrderReport")]
        public int order_report_id { get; set; }
        public int product_id { get; set; }
        public int total_sold { get; set; }
        public decimal revenue { get; set; }
        public decimal cost { get; set; }
        public decimal profit { get; set; }
        public OrderReportModel OrderReport { get; set; }
    }
}
