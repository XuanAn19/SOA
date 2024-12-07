using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Order.API.DTOs;
using Order.API.Models;
using Order.API.Repository.Interface;
using Order.API.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace Order.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // Phương thức để lấy tất cả đơn hàng
        public async Task<List<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            var orderDtos = orders.Select(order => new OrderDTO
            {
                Id = order.id,
                CustomerName = order.customer_name,
                CustomerEmail = order.customer_email,
                TotalAmount = order.total_amount,
                Status = order.status,
                CreatedAt = order.created_at,
                UpdatedAt = order.updated_at,
                OrderItems = order.OrderItems?.Select(item => new OrderItemDTO
                {
                    Id = item.id,
                    ProductId = item.product_id,
                    ProductName = item.product_name,
                    Quantity = item.quantity,
                    UnitPrice = item.unit_price,
                    TotalPrice = item.total_price
                }).ToList()
            }).ToList();

            return orderDtos;
        }

        // Phương thức để đơn hàng
        public async Task<OrderDTO> GetAllOrdersByid(int idOrder)
        {
            var orders = await _orderRepository.GetOrderByIdAsync(idOrder);
            var orderDtos = new OrderDTO
            {
                Id = orders.id,
                CustomerName = orders.customer_name,
                CustomerEmail = orders.customer_email,
                TotalAmount = orders.total_amount,
                Status = orders.status,
                CreatedAt = orders.created_at,
                UpdatedAt = orders.updated_at,
                OrderItems = orders.OrderItems?.Select(item => new OrderItemDTO
                {
                    Id = item.id,
                    ProductId = item.product_id,
                    ProductName = item.product_name,
                    Quantity = item.quantity,
                    UnitPrice = item.unit_price,
                    TotalPrice = item.total_price
                }).ToList()
            };

            return orderDtos;
        }

        // Phương thức để giải mã token và lấy thông tin người dùng
        private (string email, string fullname) GetUserInfoFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();


            var jwtToken = handler.ReadJwtToken(token);

            // Lấy các claim từ token
            var email = jwtToken?.Claims?.FirstOrDefault(c => c.Type == "Email")?.Value;
            var fullname = jwtToken?.Claims?.FirstOrDefault(c => c.Type == "Name")?.Value;

            return (email, fullname);
        }

        // Phương thức để cập nhật số lượng sản phẩm
        private async Task<bool> UpdateProductQuantitiesAsync(List<ProductUpdateDTO> productUpdates, string token)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:5001/api/products/update-quantities");
            request.Headers.Add("Authorization", "Bearer " + token);
            request.Content = new StringContent(JsonConvert.SerializeObject(productUpdates), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        public async Task<OrderModel> CreateOrderAsync(CreateOrderDTO orderDto, string token)
        {
            // Lấy thông tin người dùng từ token
            var userInfo = GetUserInfoFromToken(token);

            // Lấy danh sách sản phẩm từ service Product
            var products = await _orderRepository.GetProductsAsync(token);

            var orderItems = new List<OrderItemModel>();
            decimal totalAmount = 0;

            // Danh sách sản phẩm cần cập nhật số lượng
            var productUpdates = new List<ProductUpdateDTO>();

            foreach (var item in orderDto.OrderItems)
            {
                // Tìm sản phẩm trong danh sách sản phẩm
                var product = products.FirstOrDefault(p => p.id == item.ProductId);
                if (product == null)
                {
                    throw new Exception($"Product with ID {item.ProductId} not found.");
                }

                // Kiểm tra số lượng tồn kho
                if (product.quantity < item.Quantity)
                {
                    throw new Exception($"Product with ID {item.ProductId} does not have enough stock. Available: {product.quantity}, Required: {item.Quantity}");
                }

                // Tạo đối tượng OrderItem
                var orderItem = new OrderItemModel
                {
                    product_id = product.id,
                    product_name = product.name,
                    quantity = item.Quantity,
                    unit_price = product.price,
                    total_price = item.Quantity * product.price
                };

                // Tính tổng số tiền đơn hàng
                totalAmount += orderItem.total_price;
                orderItems.Add(orderItem);

                // Thêm sản phẩm vào danh sách cập nhật số lượng
                productUpdates.Add(new ProductUpdateDTO
                {
                    product_id = product.id,
                    NewQuantity = product.quantity - item.Quantity
                });
            }

            // Tạo đơn hàng và thêm thông tin khách hàng từ token
            var order = new OrderModel
            {
                customer_name = userInfo.fullname,
                customer_email = userInfo.email,
                total_amount = totalAmount,
                status = "Pending",
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow,
                OrderItems = orderItems
            };

            // Gửi yêu cầu cập nhật số lượng sản phẩm
            var updateSuccess = await UpdateProductQuantitiesAsync(productUpdates, token);
            if (!updateSuccess)
            {
                throw new Exception("Failed to update product quantities.");
            }

            // Lưu đơn hàng
            return await _orderRepository.CreateOrderAsync(order);
        }


        // Phương thức để cập nhật trạng thái đơn hàng
        public async Task<OrderModel> UpdateOrderStatusAsync(int id, string status)
        {
            return await _orderRepository.UpdateOrderStatusAsync(id, status);
        }

        // Phương thức để xóa đơn hàng
        public async Task<bool> DeleteOrderAsync(int id)
        {
            return await _orderRepository.DeleteOrderAsync(id);
        }
        // Phương thức để lấy tất cả mặt hàng trong đơn hàng

        /// <summary>
        /// Order detail
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<List<OrderItemModel>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            // Lấy tất cả mặt hàng trong đơn hàng theo orderId
            var orderItems = await _orderRepository.GetOrderItemsByOrderIdAsync(orderId);
            return orderItems;
        }
        // Phương thức để lấy thông tin chi tiết một mặt hàng trong đơn hàng
        public async Task<OrderItemModel> GetOrderItemByIdAsync(int idItem)
        {
            return await _orderRepository.GetOrderItemByIdAsync(idItem);
        }

        // Phương thức để tạo mặt hàng mới trong đơn hàng
        public async Task<OrderItemModel> CreateOrderItemAsync(CreateOrderItemDTO dto, string token)
        {// Kiểm tra trạng thái đơn hàng
            var orderStatus = await _orderRepository.GetOrderStatusAsync(dto.OrderId);
            if (orderStatus == null)
            {
                throw new Exception($"Order with ID {dto} not found.");
            }

            if (orderStatus == "Completed")
            {
                throw new Exception($"Cannot add items to an order unless it is in 'Completed' status. Current status: {orderStatus}.");
            }

            // Lấy thông tin sản phẩm từ ProductService
            var products = await _orderRepository.GetProductsAsync(token);
            var product = products.FirstOrDefault(p => p.id == dto.ProductId);
            if (product == null)
            {
                throw new Exception($"Product with ID {dto.ProductId} not found.");
            }

            // Tạo đối tượng OrderItem
            var orderItem = new OrderItemModel
            {
                order_id = dto.OrderId,
                product_id = product.id,
                product_name = product.name,
                quantity = dto.Quantity,
                unit_price = product.price,
                total_price = dto.Quantity * product.price
            };

            // Thêm sản phẩm vào chi tiết đơn hàng
            await _orderRepository.CreateOrderItemAsync(orderItem);

            // Cập nhật tổng tiền của đơn hàng
            await _orderRepository.UpdateOrderTotalAmountAsync(dto.OrderId, orderItem.total_price);

            return orderItem;
        }

        // Phương thức để xóa mặt hàng trong đơn hàng
        public async Task<bool> DeleteOrderItemAsync(int id)
        {
            return await _orderRepository.DeleteOrderItemAsync(id);
        }
    }
}
