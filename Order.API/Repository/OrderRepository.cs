using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Order.API.Data;
using Order.API.DTOs;
using Order.API.Models;
using Order.API.Repository.Interface;

namespace Order.API.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly HttpClient _httpClient;
        private readonly DataContext _context;

        public OrderRepository(IHttpClientFactory httpClientFactory, DataContext context)
        {
            _httpClient = httpClientFactory.CreateClient("ProductService");
            _context = context;
        }
        public async Task<List<ProductDTO>> GetProductsAsync(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "");
            request.Headers.Add("Authorization", "Bearer " + token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<ProductDTO>>(result);
            return products;

        }

        public async Task<OrderModel> CreateOrderAsync(OrderModel order)
        {
            _context.orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        // Lấy danh sách tất cả đơn hàng
        public async Task<List<OrderModel>> GetAllOrdersAsync()
        {
            return await _context.orders
                .Include(o => o.OrderItems)
                .ToListAsync();
        }
        // Lấy thông tin chi tiết một đơn hàng
        public async Task<OrderModel> GetOrderByIdAsync(int id)
        {
            return await _context.orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.id == id);
        }
        // Cập nhật trạng thái đơn hàng
        public async Task<OrderModel> UpdateOrderStatusAsync(int id, string status)
        {
            var order = await _context.orders.FindAsync(id);
            if (order != null)
            {
                order.status = status;
                await _context.SaveChangesAsync();
                return order;
            }
            return null;
        }
        // Xóa đơn hàng
        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.orders.FindAsync(id);
            if (order != null)
            {
                _context.orders.Remove(order);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Order detail
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        // Lấy tất cả mặt hàng trong đơn hàng
        public async Task<List<OrderItemModel>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            return await _context.other_items
                                 .Where(oi => oi.order_id == orderId) 
                                 .ToListAsync();
        }
        // Lấy thông tin chi tiết một mặt hàng trong đơn hàng
        public async Task<OrderItemModel> GetOrderItemByIdAsync(int idItem)
        {
            return await _context.other_items.FindAsync(idItem);
        }

        // Thêm mặt hàng mới vào đơn hàng
        public async Task<OrderItemModel> CreateOrderItemAsync(OrderItemModel orderItem)
        {
            _context.other_items.Add(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }


        // Cập nhật mặt hàng trong đơn hàng
        public async Task UpdateOrderTotalAmountAsync(int orderId, decimal additionalAmount)
        {
            var order = await _context.orders.FirstOrDefaultAsync(o => o.id == orderId);
            if (order != null)
            {
                order.total_amount += additionalAmount;
                _context.orders.Update(order);
                await _context.SaveChangesAsync();
            }
        }

        //kiểm tra trạng thái đơn hàng.
        public async Task<string> GetOrderStatusAsync(int orderId)
        {
            var order = await _context.orders.FirstOrDefaultAsync(o => o.id == orderId);
            return order?.status;
        }


        // Xóa mặt hàng trong đơn hàng
        public async Task<bool> DeleteOrderItemAsync(int id)
        {
            var orderItem = await _context.other_items.FindAsync(id);
            if (orderItem != null)
            {
                _context.other_items.Remove(orderItem);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


    }


}
