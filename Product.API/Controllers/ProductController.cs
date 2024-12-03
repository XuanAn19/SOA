using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.API.DTOs;
using Product.API.Services;
using Product.API.Services.Interface;

namespace Product.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _product;

        public ProductController(IProductService product)
        {
            _product = product;
        }
        [HttpGet]
        public async Task<ActionResult<List<ProductDTO>>> GetAllProducts()
        {
            var products = await _product.GetAllProduct();
            if (products == null || !products.Any())
            {
                return NotFound(); 
            }

            return Ok(products); 
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product =await _product.GetProductById(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductDTO product)
        {
            try
            {
                await _product.CreateProductAsync(product);
                return Ok(new
                {
                    status = 200,
                    message = "Product create successfully"
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new { status = "no", message = ex.Message });
            }

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm]ProductDTO product)
        {

            try
            {
                await _product.EditProductAsync(product, id);
                return Ok(new
                {
                    status = 200,
                    message = "Product update successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = "no", message = ex.Message });
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _product.DeleteProductAsync(id);
                return Ok(new
                {
                    status = 200,
                    message = "Product delete successfully"
                });
            } 
            catch(Exception ex)
            {
                return BadRequest(new { status = "no", message = ex.Message });
            }
        }

    }
}
