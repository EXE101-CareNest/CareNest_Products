using System.ComponentModel.DataAnnotations;
using CareNest_Products.Domain.Commons;

namespace CareNest_Products.Domain.Entities
{
    /// <summary>
    /// Entity sản phẩm chính
    /// </summary>
    public class Product : BaseEntity
    {
        /// <summary>
        /// ID của danh mục sản phẩm (Foreign Key)
        /// </summary>
        [Required]
        public string ProductCategoryId { get; set; } = string.Empty;

        /// <summary>
        /// Tên sản phẩm (1-100 ký tự)
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả sản phẩm (0-500 ký tự)
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Trạng thái sản phẩm (true: active, false: inactive)
        /// </summary>
        public bool Status { get; set; } = true;

        /// <summary>
        /// Danh sách URL hình ảnh (phân tách bằng dấu phẩy)
        /// </summary>
        [Required]
        public string ImgUrls { get; set; } = string.Empty;

        // Navigation Properties
        /// <summary>
        /// Danh mục sản phẩm chứa sản phẩm này
        /// </summary>
        public virtual ProductCategory ProductCategory { get; set; } = null!;

        /// <summary>
        /// Danh sách các chi tiết sản phẩm
        /// </summary>
        public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
    }
}
