namespace Order.API.DTOs
{
    public class CreateOrderDTO
    {
        public List<CreateOrderItemDTO> OrderItems { get; set; }
    }
}
