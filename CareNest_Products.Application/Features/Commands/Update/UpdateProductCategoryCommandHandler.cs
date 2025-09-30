using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Domain.Entities;

namespace CareNest_Products.Application.Features.Commands.Update
{
    /// <summary>
    /// Handler cập nhật danh mục sản phẩm
    /// </summary>
    public class UpdateProductCategoryCommandHandler : ICommandHandler<UpdateProductCategoryCommand, ProductCategory>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductCategory> HandleAsync(UpdateProductCategoryCommand command)
        {
            var existing = await _unitOfWork.GetRepository<ProductCategory>().GetByIdAsync(command.Id);
            if (existing == null)
            {
                throw new ArgumentException($"Không tìm thấy danh mục với ID: {command.Id}");
            }

            if (string.IsNullOrWhiteSpace(command.Name))
            {
                throw new ArgumentException("Tên danh mục không được để trống");
            }

            existing.Name = command.Name;
            existing.UpdatedAt = DateTimeOffset.UtcNow;

            await _unitOfWork.GetRepository<ProductCategory>().UpdateAsync(existing);
            await _unitOfWork.SaveAsync();

            return existing;
        }
    }
}


