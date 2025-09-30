using CareNest_Products.Application.Interfaces.CQRS.Commands;

namespace CareNest_Products.Application.Features.Commands.Delete
{
    /// <summary>
    /// Command xóa chi tiết sản phẩm
    /// </summary>
    public class DeleteProductDetailCommand : ICommand
    {
        public string Id { get; set; }
    }
}


