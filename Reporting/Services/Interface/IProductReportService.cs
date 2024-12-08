using Reporting.Models;

namespace Reporting.Services.Interface
{
    public interface IProductReportService
    {
        Task<ProductReportModel> CreateProductReportAsync(int productId, int orderReportId, string token);
        Task DeleteAsync(int id);
        Task<List<ProductReportModel>> GetAllAsync();
        Task<ProductReportModel> GetByIdAsync(int id);
    }
}
