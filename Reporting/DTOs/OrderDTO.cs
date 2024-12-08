namespace Reporting.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; } // ID của đơn hàng
        public List<OrderItemDTO> OrderItems { get; set; } // Danh sách sản phẩm trong đơn hàng
    }

    public class OrderItemDTO
    {
        public int ProductId { get; set; } // ID sản phẩm
        public int Quantity { get; set; } // Số lượng sản phẩm
        public decimal UnitPrice { get; set; } // Giá bán của sản phẩm
    }
}
