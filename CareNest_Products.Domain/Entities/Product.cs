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
        /// ID của shop (Foreign Key)
        /// </summary>
        [Required]
        public string ShopId { get; set; } = string.Empty;

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
        /// Danh sách các danh mục sản phẩm
        /// </summary>
        public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    }
}
