using Reporting.Models;
using Reporting.Repository.Interface;
using Reporting.Services.Interface;

namespace Reporting.Services
{
    public class OrderReportService : IOrderReportService
    {
        private readonly IOrderReportRepository _orderReportRepository;

        public OrderReportService(IOrderReportRepository orderReportRepository)
        {
            _orderReportRepository = orderReportRepository;
        }

        public async Task<OrderReportModel> CreateOrderReportAsync(int orderId, string token)
        {
            // Lấy thông tin đơn hàng từ dịch vụ Quản lý đơn hàng
            var order = await _orderReportRepository.GetOrderByIdAsync(orderId, token);

            // Tính toán tổng doanh thu, tổng chi phí và lợi nhuận
            var totalRevenue = order.OrderItems.Sum(item => item.Quantity * item.UnitPrice);
            var totalCost = order.OrderItems.Sum(item => item.Quantity * (item.UnitPrice + (item.UnitPrice*(decimal)0.3) ));
            var totalProfit = totalRevenue - totalCost;

            // Tạo báo cáo đơn hàng
            var report = new OrderReportModel
            {
                order_id = order.Id,
                total_revenue = totalRevenue,
                total_cost = totalCost,
                total_profit = totalProfit
            };

            return await _orderReportRepository.AddAsync(report);
        }

        public async Task<List<OrderReportModel>> GetAllAsync()
        {
            return await _orderReportRepository.GetAllAsync();
        }

        public async Task<OrderReportModel> GetByIdAsync(int id)
        {
            return await _orderReportRepository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _orderReportRepository.DeleteAsync(id);
        }
    }
}
