using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Reporting.Data;
using Reporting.DTOs;
using Reporting.Models;
using Reporting.Repository.Interface;
using System.Net.Http;

namespace Reporting.Repository
{
    public class OrderReportRepository : IOrderReportRepository
    {
        private readonly HttpClient _orderServiceClient;
        private readonly DataContext _context;

        public OrderReportRepository(IHttpClientFactory httpClientFactory, DataContext context)
        {
            _orderServiceClient = httpClientFactory.CreateClient("OrderService");
            _context = context;
        }

        // Lấy thông tin đơn hàng từ dịch vụ Quản lý đơn hàng
        public async Task<OrderDTO> GetOrderByIdAsync(int orderId, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/orders/{orderId}");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await _orderServiceClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OrderDTO>(content);
        }

        public async Task<List<OrderItemDTO>> GetOrderItemsByProductIdAsync(int productId, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/order_items?product_id={productId}");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await _orderServiceClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<OrderItemDTO>>(result);
        }

        public async Task<OrderReportModel> AddAsync(OrderReportModel report)
        {
            _context.orders_reports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }
        public async Task<List<OrderReportModel>> GetAllAsync()
        {
            return await _context.orders_reports.Include(o => o.ProductReports).ToListAsync();
        }

        public async Task<OrderReportModel> GetByIdAsync(int id)
        {
            return await _context.orders_reports.Include(o => o.ProductReports).FirstOrDefaultAsync(o => o.id == id);
        }


        public async Task DeleteAsync(int id)
        {
            var report = await _context.orders_reports.FindAsync(id);
            if (report != null)
            {
                _context.orders_reports.Remove(report);
                await _context.SaveChangesAsync();
            }
        }
    }
}
