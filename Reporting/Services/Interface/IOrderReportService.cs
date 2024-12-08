using Reporting.Models;

namespace Reporting.Services.Interface
{
    public interface IOrderReportService
    {
        Task<OrderReportModel> CreateOrderReportAsync(int orderId, string token);
        Task DeleteAsync(int id);
        Task<List<OrderReportModel>> GetAllAsync();
        Task<OrderReportModel> GetByIdAsync(int id);
    }
}
