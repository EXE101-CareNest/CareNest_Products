using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Domain.Entities;
using System.Text.Json.Serialization;

namespace CareNest_Products.Application.Features.Commands.Create
{
    /// <summary>
    /// Command tạo mới danh mục sản phẩm
    /// </summary>
    public class CreateProductCategoryCommand : ICommand<ProductCategory>
    {
        /// <summary>
        /// ID của shop
        /// </summary>
        public string ShopId { get; set; } = string.Empty;

        /// <summary>
        /// Tên danh mục/phân loại
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
