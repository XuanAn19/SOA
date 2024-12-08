using Reporting.Models;
using Reporting.Repository.Interface;
using Reporting.Services.Interface;

namespace Reporting.Services
{
    public class ProductReportService : IProductReportService
    {
        private readonly IProductReportRepository _productReportRepository;
        private readonly IOrderReportRepository _orderRepository;

        public ProductReportService(IProductReportRepository productReportRepository, IOrderReportRepository orderReportRepository)
        {
            _productReportRepository = productReportRepository;
            _orderRepository = orderReportRepository;
        }

        public async Task<ProductReportModel> CreateProductReportAsync(int productId, int orderReportId, string token)
        {
            // Lấy thông tin sản phẩm từ ProductService
            var product = await _productReportRepository.GetProductByIdAsync(productId, token);

            // Lấy thông tin các đơn hàng có chứa sản phẩm này từ OrderService
            var orderItems = await _orderRepository.GetOrderItemsByProductIdAsync(productId, token);

            // Tính tổng số lượng sản phẩm đã bán
            var totalSold = orderItems.Sum(item => item.Quantity);

            // Tính doanh thu, chi phí và lợi nhuận
            var revenue = totalSold * product.price;
            var cost = totalSold * (product.price -product.price*(decimal)0.3);
            var profit = revenue - cost;

            // Tạo báo cáo sản phẩm
            var report = new ProductReportModel
            {
                order_report_id = orderReportId,
                product_id = product.id,
                total_sold = totalSold,
                revenue = revenue,
                cost = cost,
                profit = profit
            };

            // Lưu báo cáo vào cơ sở dữ liệu
            return await _productReportRepository.AddAsync(report);
        }

        public async Task<List<ProductReportModel>> GetAllAsync()
        {
            return await _productReportRepository.GetAllAsync();
        }

        public async Task<ProductReportModel> GetByIdAsync(int id)
        {
            return await _productReportRepository.GetByIdAsync(id);
        }

        

        public async Task DeleteAsync(int id)
        {
            await _productReportRepository.DeleteAsync(id);
        }

    }
}
