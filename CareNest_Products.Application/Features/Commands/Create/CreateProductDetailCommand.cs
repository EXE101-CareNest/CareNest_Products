using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Domain.Entities;
using System.Text.Json.Serialization;

namespace CareNest_Products.Application.Features.Commands.Create
{
    /// <summary>
    /// Command tạo mới chi tiết sản phẩm
    /// </summary>
    public class CreateProductDetailCommand : ICommand<ProductDetail>
    {
        /// <summary>
        /// ID của sản phẩm
        /// </summary>
        public string ProductId { get; set; } = string.Empty;

        /// <summary>
        /// Tên chi tiết cụ thể
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Giá sản phẩm
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// Trạng thái kho (true: còn hàng, false: hết hàng)
        /// </summary>
        public bool Status { get; set; } = true;

        /// <summary>
        /// Phần trăm giảm giá
        /// </summary>
        public double? Discount { get; set; }

        /// <summary>
        /// Đánh dấu mặc định
        /// </summary>
        public bool IsDefault { get; set; } = false;

        /// <summary>
        /// Hình ảnh riêng cho chi tiết này
        /// </summary>
        public string? ImgUrls { get; set; }

        /// <summary>
        /// Số lượng tồn kho
        /// </summary>
        public int QuantityInStock { get; set; }
    }
}
