namespace Order.API.DTOs
{
    public class CreateOrderItemDTO
    {
        public int ProductId { get; set; }     // ID sản phẩm
        public int Quantity { get; set; } // số lượng sản phẩm
    }
}
