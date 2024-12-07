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
        Task<OrderItemModel> CreateOrderItemAsync(OrderItemModel orderItem);
        Task<List<OrderItemModel>> GetOrderItemsByOrderIdAsync(int orderId);
        Task<List<ProductDTO>> GetProductsAsync(string token);
        Task<OrderModel> UpdateOrderStatusAsync(int id, string status);
        Task<bool> DeleteOrderItemAsync(int id);
        Task UpdateOrderTotalAmountAsync(int orderId, decimal additionalAmount);
        Task<string> GetOrderStatusAsync(int orderId);
    }
}
