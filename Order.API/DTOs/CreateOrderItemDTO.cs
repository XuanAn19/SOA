namespace Order.API.DTOs
{
    public class CreateOrderItemDTO
    {
        public int OrderId { get; set; }      // ID của đơn hàng
        public int ProductId { get; set; }    // ID sản phẩm
        public int Quantity { get; set; }     // Số lượng sản phẩm
    }
}
