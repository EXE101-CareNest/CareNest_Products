using CareNest_Products.Application.Features.Queries.GetAllPaging;

namespace CareNest_Products.Application.Features.Queries.GetById
{
    /// <summary>
    /// DTO sản phẩm kèm danh mục con
    /// </summary>
    public class ProductWithCategoriesResponse
    {
        public string Id { get; set; }
        public string ShopId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Status { get; set; }
        public string ImgUrls { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public List<ProductCategoryResponse> Categories { get; set; } = new();
    }
}


