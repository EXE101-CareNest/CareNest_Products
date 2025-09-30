using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Domain.Entities;

namespace CareNest_Products.Application.Features.Commands.Update
{
    /// <summary>
    /// Handler cập nhật chi tiết sản phẩm
    /// </summary>
    public class UpdateProductDetailCommandHandler : ICommandHandler<UpdateProductDetailCommand, ProductDetail>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductDetailCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductDetail> HandleAsync(UpdateProductDetailCommand command)
        {
            var repo = _unitOfWork.GetRepository<ProductDetail>();
            var existing = await repo.GetByIdAsync(command.Id);
            if (existing == null)
            {
                throw new ArgumentException($"Không tìm thấy chi tiết với ID: {command.Id}");
            }

            if (string.IsNullOrWhiteSpace(command.Name))
            {
                throw new ArgumentException("Tên chi tiết không được để trống");
            }

            existing.Name = command.Name;
            existing.Price = command.Price;
            existing.Status = command.Status;
            existing.Discount = command.Discount;
            existing.IsDefault = command.IsDefault;
            existing.ImgUrls = command.ImgUrls;
            existing.QuantityInStock = command.QuantityInStock;
            existing.UpdatedAt = DateTimeOffset.UtcNow;

            await repo.UpdateAsync(existing);
            await _unitOfWork.SaveAsync();

            return existing;
        }
    }
}


