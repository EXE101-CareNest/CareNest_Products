namespace CareNest_Products.Application.Features.Queries.GetAllPaging
{
    /// <summary>
    /// Response DTO cho ProductDetail
    /// </summary>
    public class ProductDetailResponse
    {
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public bool Status { get; set; }
        public double? Discount { get; set; }
        public bool IsDefault { get; set; }
        public string? ImgUrls { get; set; }
        public int QuantityInStock { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
