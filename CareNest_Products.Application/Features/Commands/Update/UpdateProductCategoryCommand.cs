using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Domain.Entities;
using System.Text.Json.Serialization;

namespace CareNest_Products.Application.Features.Commands.Update
{
    /// <summary>
    /// Command cập nhật danh mục sản phẩm
    /// </summary>
    public class UpdateProductCategoryCommand : ICommand<ProductCategory>
    {
        /// <summary>
        /// ID danh mục cần cập nhật
        /// </summary>
        [JsonIgnore]
        public string? Id { get; set; }

        /// <summary>
        /// Tên danh mục
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}


