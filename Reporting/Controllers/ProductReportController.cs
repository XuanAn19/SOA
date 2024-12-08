using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reporting.DTOs;
using Reporting.Services.Interface;

namespace Reporting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductReportController : ControllerBase
    {
        private readonly IProductReportService _productReportService;

        public ProductReportController(IProductReportService productReportService)
        {
            _productReportService = productReportService;
        }

        // POST: api/productreports
        [HttpPost]
        public async Task<IActionResult> CreateProductReport([FromBody] CreateProductReportDTO reportDto)
        {
            try
            {
                // Lấy token từ header Authorization
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                // Gọi service để tạo báo cáo sản phẩm
                var report = await _productReportService.CreateProductReportAsync(
                    reportDto.ProductId,
                    reportDto.OrderReportId,
                    token
                );

                // Trả về thông báo thành công và ID của báo cáo vừa tạo
                return Ok(new { message = "Product report created successfully.", reportId = report.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/productreports
        [HttpGet]
        public async Task<IActionResult> GetAllProductReports()
        {
            try
            {
                var reports = await _productReportService.GetAllAsync();
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/productreports/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductReportById(int id)
        {
            try
            {
                var report = await _productReportService.GetByIdAsync(id);
                if (report == null)
                {
                    return NotFound(new { message = "Product report not found." });
                }

                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // DELETE: api/productreports/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductReport(int id)
        {
            try
            {
                // Gọi service để xóa báo cáo sản phẩm
                await _productReportService.DeleteAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
