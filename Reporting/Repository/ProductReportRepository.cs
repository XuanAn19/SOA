using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Reporting.Data;
using Reporting.DTOs;
using Reporting.Models;
using Reporting.Repository.Interface;

namespace Reporting.Repository
{
    public class ProductReportRepository : IProductReportRepository
    {
        private readonly HttpClient _productServiceClient;
        private readonly DataContext _context;

        public ProductReportRepository(IHttpClientFactory httpClientFactory, DataContext context)
        {
            _productServiceClient = httpClientFactory.CreateClient("ProductService");
            _context = context;
        }

        // Lấy thông tin sản phẩm từ dịch vụ Quản lý sản phẩm
        public async Task<ProductDTO> GetProductByIdAsync(int productId, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/products/{productId}");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await _productServiceClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProductDTO>(content);
        }

        public async Task<ProductReportModel> AddAsync(ProductReportModel report)
        {
            _context.product_reports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<List<ProductReportModel>> GetAllAsync()
        {
            return await _context.product_reports.ToListAsync();
        }

        public async Task<ProductReportModel> GetByIdAsync(int id)
        {
            return await _context.product_reports.FirstOrDefaultAsync(p => p.Id == id);
        }


        public async Task DeleteAsync(int id)
        {
            var report = await _context.product_reports.FindAsync(id);
            if (report != null)
            {
                _context.product_reports.Remove(report);
                await _context.SaveChangesAsync();
            }
        }
    }
}
