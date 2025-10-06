namespace CareNest_Products.Application.Features.Queries.GetAllPaging
{
    /// <summary>
    /// Response DTO cho Product
    /// </summary>
    public class ProductResponse
    {
        public string Id { get; set; }
        public string ProductCategoryId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Status { get; set; }
        public string ImgUrls { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
