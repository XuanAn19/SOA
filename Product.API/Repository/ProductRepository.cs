using Microsoft.EntityFrameworkCore;
using Product.API.Data;
using Product.API.Models;
using Product.API.Repository.Interface;

namespace Product.API.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _dataContext;
        public ProductRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<List<ProductModel>> GetAll()
        {
            return await _dataContext.Products.ToListAsync();
        }

        public async Task AddProductAsync(ProductModel product)
        {
            _dataContext.Products.Add(product);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<ProductModel> GetProductById(int id)
        {
            return await _dataContext.Products.Where(p=>p.id==id).FirstOrDefaultAsync();
        } 

        public async Task UpdateProductAsync(ProductModel product)
        {
            _dataContext.Products.Update(product);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(ProductModel product)
        {
            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
        }


    }
}
