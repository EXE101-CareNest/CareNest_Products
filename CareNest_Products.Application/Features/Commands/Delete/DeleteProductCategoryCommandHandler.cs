using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Domain.Entities;

namespace CareNest_Products.Application.Features.Commands.Delete
{
    /// <summary>
    /// Handler xóa danh mục sản phẩm
    /// </summary>
    public class DeleteProductCategoryCommandHandler : ICommandHandler<DeleteProductCategoryCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(DeleteProductCategoryCommand command)
        {
            var repo = _unitOfWork.GetRepository<ProductCategory>();
            var entity = await repo.GetByIdAsync(command.Id);
            if (entity == null)
            {
                throw new ArgumentException($"Không tìm thấy danh mục với ID: {command.Id}");
            }

            await repo.DeleteAsync(entity);
            await _unitOfWork.SaveAsync();
        }
    }
}


