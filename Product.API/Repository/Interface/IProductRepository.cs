using Product.API.Models;

namespace Product.API.Repository.Interface
{
    public interface IProductRepository
    {
        Task AddProductAsync(ProductModel product);
        Task DeleteProductAsync(ProductModel product);
        Task<List<ProductModel>> GetAll();
        Task<ProductModel> GetProductById(int id);
        Task UpdateProductAsync(ProductModel product);
    }
}
