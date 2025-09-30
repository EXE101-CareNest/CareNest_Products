using System.ComponentModel.DataAnnotations;
using CareNest_Products.Domain.Commons;

namespace CareNest_Products.Domain.Entities
{
    /// <summary>
    /// Entity chi tiết sản phẩm
    /// </summary>
    public class ProductDetail : BaseEntity
    {
        /// <summary>
        /// ID của danh mục sản phẩm (Foreign Key)
        /// </summary>
        [Required]
        public string CategoryId { get; set; }

        /// <summary>
        /// Tên chi tiết cụ thể
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Giá sản phẩm (phải > 0)
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0")]
        public int Price { get; set; }

        /// <summary>
        /// Trạng thái kho (true: còn hàng, false: hết hàng)
        /// </summary>
        [Required]
        public bool Status { get; set; } = true;

        /// <summary>
        /// Phần trăm giảm giá (0-100)
        /// </summary>
        [Range(0, 100, ErrorMessage = "Phần trăm giảm giá phải từ 0 đến 100")]
        public double? Discount { get; set; }

        /// <summary>
        /// Đánh dấu mặc định
        /// </summary>
        [Required]
        public bool IsDefault { get; set; } = false;

        /// <summary>
        /// Hình ảnh riêng cho chi tiết này
        /// </summary>
        [StringLength(1000)]
        public string? ImgUrls { get; set; }

        /// <summary>
        /// Số lượng tồn kho (phải >= 0)
        /// </summary>
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho phải >= 0")]
        public int QuantityInStock { get; set; }

        // Navigation Properties
        /// <summary>
        /// Danh mục sản phẩm chứa chi tiết này
        /// </summary>
        public virtual ProductCategory ProductCategory { get; set; } = null!;
    }
}
