using Reporting.DTOs;
using Reporting.Models;

namespace Reporting.Repository.Interface
{
    public interface IProductReportRepository
    {
        Task<ProductDTO> GetProductByIdAsync(int productId, string token);
        Task<ProductReportModel> AddAsync(ProductReportModel report);
        Task DeleteAsync(int id);
        Task<List<ProductReportModel>> GetAllAsync();
        Task<ProductReportModel> GetByIdAsync(int id);
    }
}
