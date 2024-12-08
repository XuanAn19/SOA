using Reporting.DTOs;
using Reporting.Models;

namespace Reporting.Repository.Interface
{
    public interface IOrderReportRepository
    {
        Task<OrderReportModel> AddAsync(OrderReportModel report);
        Task DeleteAsync(int id);
        Task<List<OrderReportModel>> GetAllAsync();
        Task<OrderReportModel> GetByIdAsync(int id);
        Task<OrderDTO> GetOrderByIdAsync(int orderId, string token);
        Task<List<OrderItemDTO>> GetOrderItemsByProductIdAsync(int productId, string token);
    }
}
