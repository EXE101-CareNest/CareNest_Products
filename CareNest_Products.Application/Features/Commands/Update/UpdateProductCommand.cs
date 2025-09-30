using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Domain.Entities;

namespace CareNest_Products.Application.Features.Commands.Update
{
    /// <summary>
    /// Command cập nhật sản phẩm
    /// </summary>
    public class UpdateProductCommand : ICommand<Product>
    {
        /// <summary>
        /// ID của sản phẩm cần cập nhật
        /// </summary>
        public string Id { get; set; }

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
