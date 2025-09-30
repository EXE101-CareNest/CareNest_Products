using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Domain.Entities;

namespace CareNest_Products.Application.Features.Commands.Create
{
    /// <summary>
    /// Command tạo mới danh mục sản phẩm
    /// </summary>
    public class CreateProductCategoryCommand : ICommand<ProductCategory>
    {
        /// <summary>
        /// ID của sản phẩm
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Tên danh mục/phân loại
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
