using CareNest_Products.Application.Interfaces.CQRS.Commands;

namespace CareNest_Products.Application.Features.Commands.Delete
{
    /// <summary>
    /// Command xóa sản phẩm
    /// </summary>
    public class DeleteProductCommand : ICommand
    {
        /// <summary>
        /// ID của sản phẩm cần xóa
        /// </summary>
        public string Id { get; set; }
    }
}
