using System.ComponentModel.DataAnnotations;

namespace CareNest_Products.Application.Features.Commands.Create
{
    /// <summary>
    /// Request DTO để tạo mới danh mục sản phẩm (không chứa ShopId vì lấy từ URL)
    /// </summary>
    public class CreateProductCategoryRequest
    {
        /// <summary>
        /// Tên danh mục/phân loại
        /// </summary>
        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(200, ErrorMessage = "Tên danh mục không được vượt quá 200 ký tự")]
        public string Name { get; set; } = string.Empty;
    }
}
