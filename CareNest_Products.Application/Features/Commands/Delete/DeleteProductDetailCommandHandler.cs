using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Domain.Entities;

namespace CareNest_Products.Application.Features.Commands.Delete
{
    /// <summary>
    /// Handler xóa chi tiết sản phẩm
    /// </summary>
    public class DeleteProductDetailCommandHandler : ICommandHandler<DeleteProductDetailCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductDetailCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(DeleteProductDetailCommand command)
        {
            var repo = _unitOfWork.GetRepository<ProductDetail>();
            var entity = await repo.GetByIdAsync(command.Id);
            if (entity == null)
            {
                throw new ArgumentException($"Không tìm thấy chi tiết với ID: {command.Id}");
            }

            await repo.DeleteAsync(entity);
            await _unitOfWork.SaveAsync();
        }
    }
}


