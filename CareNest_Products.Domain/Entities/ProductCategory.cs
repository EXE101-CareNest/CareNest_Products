using System.ComponentModel.DataAnnotations;
using CareNest_Products.Domain.Commons;

namespace CareNest_Products.Domain.Entities
{
    /// <summary>
    /// Entity phân loại sản phẩm
    /// </summary>
    public class ProductCategory : BaseEntity
    {
        /// <summary>
        /// ID của sản phẩm (Foreign Key)
        /// </summary>
        [Required]
        public string ProductId { get; set; }

        /// <summary>
        /// Tên danh mục/phân loại
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        // Navigation Properties
        /// <summary>
        /// Sản phẩm chứa danh mục này
        /// </summary>
        public virtual Product Product { get; set; } = null!;

        /// <summary>
        /// Danh sách các chi tiết sản phẩm trong danh mục này
        /// </summary>
        public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
    }
}
