using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Product.API.DTOs;
using Product.API.Models;
using Product.API.Repository.Interface;
using Product.API.Services.Interface;

namespace Product.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }/*
        public IEnumerable<ProductModel> GetAllProduct()
        {
            var product = _productRepository.GetAll();
            if (product == null)
            {
                throw new InvalidOperationException("Product not found or unauthorized access");
            }
            return product;
        }*/
        public async Task<List<ProductModel>> GetAllProduct()
        {
            var product = await _productRepository.GetAll();
            if(product == null)
            {
                throw new InvalidOperationException("Product not found or unauthorized access");
            }
            return product;
        }
        public async Task<ProductModel> GetProductById(int id)
        {
            var product = await _productRepository.GetProductById(id);

            if (product == null)
            {
                throw new InvalidOperationException("Product not found or unauthorized access");
            }
            return product;
        }

        public async Task CreateProductAsync(ProductDTO product)
        {
            var productNew = _mapper.Map<ProductModel>(product);
            productNew.updated_at = DateTime.Now;
            productNew.created_at = DateTime.Now;
            await _productRepository.AddProductAsync(productNew);
        }

        public async Task DeleteProductAsync(int id)
        {
            var getProductById = await _productRepository.GetProductById(id);
            if(getProductById == null)
            {
                throw new InvalidOperationException("Product not found or unauthorized access");
            }
            await _productRepository.DeleteProductAsync(getProductById);
        }

        public async Task EditProductAsync([FromForm]ProductDTO product, int id)
        {
            var getProductById = await _productRepository.GetProductById(id);
            if (getProductById == null)
            {
                throw new InvalidOperationException("Product not found or unauthorized access");
            }
            var update = _mapper.Map(product, getProductById);

            update.name = product.name ?? getProductById.name;
            update.description = product.description ?? getProductById.description;
            update.price = product.price != 0 ? product.price : getProductById.price;
            update.quantity = product.quantity != 0 ? product.quantity : getProductById.quantity;


            update.updated_at = DateTime.Now;
            await _productRepository.UpdateProductAsync(update);

        }
    }
}
