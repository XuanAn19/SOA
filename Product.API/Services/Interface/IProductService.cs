using Product.API.DTOs;
using Product.API.Models;

namespace Product.API.Services.Interface
{
    public interface IProductService
    {
        Task CreateProductAsync(ProductDTO product);
        Task DeleteProductAsync(int id);
        Task EditProductAsync(ProductDTO product, int id);
        Task<List<ProductModel>> GetAllProduct();
        Task<ProductModel> GetProductById(int id);
        Task UpdateProductQuantitiesAsync(List<ProductUpdateDTO> productUpdates);
    }
}
