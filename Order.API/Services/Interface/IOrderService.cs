using Order.API.DTOs;
using Order.API.Models;

namespace Order.API.Services.Interface
{
    public interface IOrderService
    {
        Task<OrderModel> CreateOrderAsync(CreateOrderDTO orderDto, string token);
        Task<OrderItemModel> CreateOrderItemAsync(CreateOrderItemDTO dto, string token);
        Task<bool> DeleteOrderAsync(int id);
        Task<bool> DeleteOrderItemAsync(int id);
        Task<List<OrderDTO>> GetAllOrdersAsync();
        Task<OrderDTO> GetAllOrdersByid(int idOrder);
        Task<OrderItemModel> GetOrderItemByIdAsync(int id);
        Task<List<OrderItemModel>> GetOrderItemsByOrderIdAsync(int orderId);
        Task<OrderModel> UpdateOrderStatusAsync(int id, string status);
    }

}
