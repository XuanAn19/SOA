using Order.API.DTOs;
using Order.API.Models;

namespace Order.API.Repository.Interface
{
    public interface IOrderRepository
    {
        Task<OrderModel> CreateOrderAsync(OrderModel order);
        Task<bool> DeleteOrderAsync(int id);
        Task<List<OrderModel>> GetAllOrdersAsync();
        Task<OrderModel> GetOrderByIdAsync(int id);
        Task<OrderItemModel> GetOrderItemByIdAsync(int id);
        Task<List<OrderItemModel>> GetOrderItemsByOrderIdAsync(int orderId);
        Task<List<ProductDTO>> GetProductsAsync(string token);
        Task<OrderModel> UpdateOrderStatusAsync(int id, string status);
    }
}
