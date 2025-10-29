using Microsoft.AspNetCore.Http;

namespace CareNest_Products.API.Controllers.Models
{
    public class CreateProductFormModel
    {
        public string ProductCategoryId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Status { get; set; } = true;
        public IFormFile? ImageFile { get; set; }
    }
}


