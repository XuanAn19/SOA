using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Order.API.Data;
using Order.API.DTOs;
using Order.API.Services.Interface;
using System.Net.Http;

namespace Order.API.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO orderDto)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var order = await _orderService.CreateOrderAsync(orderDto, token);
                return Ok(new { message = "Order created successfully.", orderId = order.id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetAllOrdersByid(id);
                if (order == null)
                {
                    return NotFound(new { message = "Order not found." });
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDTO statusDto)
        {
            try
            {
                var order = await _orderService.UpdateOrderStatusAsync(id, statusDto.Status);
                if (order == null)
                {
                    return NotFound(new { message = "Order not found." });
                }
                return Ok(new { message = "Order status updated successfully.", orderId = order.id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var result = await _orderService.DeleteOrderAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "Order not found." });
                }
                return Ok(new { message = "Order deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Order detail steep
        /// </summary>
        /// <returns></returns>

        [HttpGet("order_items/{idOrder}/item")]
        public async Task<IActionResult> GetAllOrderItems(int idOrder)
        {
            try
            {
                var orderItems = await _orderService.GetOrderItemsByOrderIdAsync(idOrder);
                return Ok(orderItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpGet("order_items/{idItem}")]
        public async Task<IActionResult> GetOrderItemById(int idItem)
        {
            try
            {
                var orderItem = await _orderService.GetOrderItemByIdAsync(idItem);
                if (orderItem == null)
                {
                    return NotFound(new { message = "Order item not found." });
                }
                return Ok(orderItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /*//POST /order_items: Tạo mặt hàng mới trong đơn hàng.
        [HttpPost("order_items")]
        public async Task<IActionResult> CreateOrderItem([FromBody] CreateOrderItemDTO orderItemDto)
        {
            try
            {
                var orderItem = await _orderService.CreateOrderItemAsync(orderItemDto);
                return Ok(new { message = "Order item created successfully.", orderItemId = orderItem.id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }*/
       /* [HttpDelete("order_items/{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            try
            {
                var result = await _orderService.DeleteOrderItemAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "Order item not found." });
                }
                return Ok(new { message = "Order item deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }*/






        /*[HttpGet("CallProductService")]
        public async Task<IActionResult> CallProductService()
        {
            var token = Request.Headers["Authorization"].ToString();
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.GetAsync("");
            response.EnsureSuccessStatusCode();
            var products = await response.Content.ReadAsStringAsync();
            return Ok(products);
        }*/


        [HttpGet("CallProductService")]
        public async Task<ActionResult> CallProductService()
        {
            var token = Request.Headers["Authorization"].ToString();

            var client = new HttpClient();
  
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:5001/api/products");
            request.Headers.Add("Authorization", token);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
            var product = JsonConvert.DeserializeObject<List<ProductDTO>>(result);
            return Ok(product);
        }
    }
}
