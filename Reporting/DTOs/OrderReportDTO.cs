namespace Reporting.DTOs
{
    public class OrderReportDTO
    {
        public int Id { get; set; } // ID của báo cáo đơn hàng
        public int OrderId { get; set; } // ID của đơn hàng liên kết
        public decimal TotalRevenue { get; set; } // Tổng doanh thu
        public decimal TotalCost { get; set; } // Tổng chi phí
        public decimal TotalProfit { get; set; } // Tổng lợi nhuận (Revenue - Cost)
    }
}
