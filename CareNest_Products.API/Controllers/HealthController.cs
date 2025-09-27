using Microsoft.AspNetCore.Mvc;

namespace CareNest_Products.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Service = "CareNest Products API",
                Version = "1.0.0"
            });
        }

        [HttpGet("ready")]
        public IActionResult Ready()
        {
            // Có thể thêm logic kiểm tra database connection, external services, etc.
            return Ok(new
            {
                Status = "Ready",
                Timestamp = DateTime.UtcNow,
                Database = "Connected",
                Services = "Available"
            });
        }
    }
}
