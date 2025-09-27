namespace CareNest_Products.Application.Features.Queries.GetAllPaging
{
    /// <summary>
    /// Response DTO cho ProductCategory
    /// </summary>
    public class ProductCategoryResponse
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
