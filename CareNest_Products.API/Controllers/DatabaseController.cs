using Microsoft.AspNetCore.Mvc;
using CareNest_Products.Infrastructure.Persistences.Database;
using Microsoft.EntityFrameworkCore;

namespace CareNest_Products.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DatabaseController> _logger;

        public DatabaseController(ApplicationDbContext context, ILogger<DatabaseController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Seed database với dữ liệu mẫu
        /// </summary>
        /// <returns></returns>
        [HttpPost("seed")]
        public async Task<IActionResult> SeedDatabase()
        {
            try
            {
                _logger.LogInformation("Bắt đầu seed database...");
                
                await DatabaseSeeder.SeedAsync(_context);
                
                _logger.LogInformation("Seed database thành công!");
                
                return Ok(new
                {
                    success = true,
                    message = "Database đã được seed thành công với dữ liệu mẫu",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi seed database");
                
                return StatusCode(500, new
                {
                    success = false,
                    message = "Có lỗi xảy ra khi seed database",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Kiểm tra trạng thái database và số lượng dữ liệu
        /// </summary>
        /// <returns></returns>
        [HttpGet("status")]
        public async Task<IActionResult> GetDatabaseStatus()
        {
            try
            {
                var productCount = await _context.Products.CountAsync();
                var categoryCount = await _context.ProductCategories.CountAsync();
                var productDetailCount = await _context.ProductDetails.CountAsync();

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        products = productCount,
                        categories = categoryCount,
                        productDetails = productDetailCount,
                        isSeeded = productCount > 0
                    },
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi kiểm tra trạng thái database");
                
                return StatusCode(500, new
                {
                    success = false,
                    message = "Có lỗi xảy ra khi kiểm tra trạng thái database",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Xóa tất cả dữ liệu trong database (chỉ dùng cho development)
        /// </summary>
        /// <returns></returns>
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearDatabase()
        {
            try
            {
                // Chỉ cho phép trong môi trường development
                if (!HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Chức năng này chỉ được phép sử dụng trong môi trường development"
                    });
                }

                _logger.LogWarning("Bắt đầu xóa tất cả dữ liệu trong database...");

                // Xóa theo thứ tự để tránh lỗi foreign key constraint
                _context.ProductDetails.RemoveRange(_context.ProductDetails);
                _context.Products.RemoveRange(_context.Products);
                _context.ProductCategories.RemoveRange(_context.ProductCategories);

                await _context.SaveChangesAsync();

                _logger.LogWarning("Đã xóa tất cả dữ liệu trong database");

                return Ok(new
                {
                    success = true,
                    message = "Đã xóa tất cả dữ liệu trong database",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa dữ liệu database");
                
                return StatusCode(500, new
                {
                    success = false,
                    message = "Có lỗi xảy ra khi xóa dữ liệu database",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
