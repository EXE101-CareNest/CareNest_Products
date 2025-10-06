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
        /// ID của shop (Foreign Key)
        /// </summary>
        [Required]
        public string ShopId { get; set; }

        /// <summary>
        /// Tên danh mục/phân loại
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        // Navigation Properties
        /// <summary>
        /// Danh sách các sản phẩm trong danh mục này
        /// </summary>
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
