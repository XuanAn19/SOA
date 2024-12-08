namespace Reporting.DTOs
{
    public class ProductReportDTO
    {
        public int Id { get; set; } // ID của báo cáo sản phẩm
        public int OrderReportId { get; set; } // ID báo cáo đơn hàng liên kết
        public int ProductId { get; set; } // ID sản phẩm liên kết
        public int TotalSold { get; set; } // Tổng số lượng sản phẩm đã bán
        public decimal Revenue { get; set; } // Doanh thu từ sản phẩm
        public decimal Cost { get; set; } // Chi phí nhập sản phẩm
        public decimal Profit { get; set; } // Lợi nhuận (Revenue - Cost)
    }
}
