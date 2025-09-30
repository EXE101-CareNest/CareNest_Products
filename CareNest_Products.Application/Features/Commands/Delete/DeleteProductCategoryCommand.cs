using CareNest_Products.Application.Interfaces.CQRS.Commands;

namespace CareNest_Products.Application.Features.Commands.Delete
{
    /// <summary>
    /// Command xóa danh mục sản phẩm
    /// </summary>
    public class DeleteProductCategoryCommand : ICommand
    {
        public string Id { get; set; }
    }
}


