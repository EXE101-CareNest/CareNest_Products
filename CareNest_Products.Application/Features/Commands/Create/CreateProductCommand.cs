using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Domain.Entities;

namespace CareNest_Products.Application.Features.Commands.Create
{
    /// <summary>
    /// Command tạo mới sản phẩm
    /// </summary>
    public class CreateProductCommand : ICommand<Product>
    {
        /// <summary>
        /// ID của shop (chấp nhận chuỗi GUID có/không có dấu gạch)
        /// </summary>
        public string ShopId { get; set; } = string.Empty;

        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả sản phẩm
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Trạng thái sản phẩm
        /// </summary>
        public bool Status { get; set; } = true;

        /// <summary>
        /// Danh sách URL hình ảnh
        /// </summary>
        public string ImgUrls { get; set; } = string.Empty;
    }
}
