using System.ComponentModel.DataAnnotations;

namespace CareNest_Products.Application.Features.Commands.Create
{
    /// <summary>
    /// Request DTO để tạo mới chi tiết sản phẩm (không chứa ProductId vì lấy từ URL)
    /// </summary>
    public class CreateProductDetailRequest
    {
        /// <summary>
        /// Tên chi tiết cụ thể
        /// </summary>
        [Required(ErrorMessage = "Tên chi tiết không được để trống")]
        [StringLength(200, ErrorMessage = "Tên chi tiết không được vượt quá 200 ký tự")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Giá sản phẩm
        /// </summary>
        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0")]
        public int Price { get; set; }

        /// <summary>
        /// Trạng thái kho (true: còn hàng, false: hết hàng)
        /// </summary>
        public bool Status { get; set; } = true;

        /// <summary>
        /// Phần trăm giảm giá
        /// </summary>
        [Range(0, 100, ErrorMessage = "Phần trăm giảm giá phải từ 0 đến 100")]
        public double? Discount { get; set; }

        /// <summary>
        /// Đánh dấu mặc định
        /// </summary>
        public bool IsDefault { get; set; } = false;

        /// <summary>
        /// Hình ảnh riêng cho chi tiết này
        /// </summary>
        [StringLength(1000, ErrorMessage = "URL hình ảnh không được vượt quá 1000 ký tự")]
        public string? ImgUrls { get; set; }

        /// <summary>
        /// Số lượng tồn kho
        /// </summary>
        [Required(ErrorMessage = "Số lượng tồn kho không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho phải >= 0")]
        public int QuantityInStock { get; set; }
    }
}
