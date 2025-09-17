using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Domain.Entities;

namespace CareNest_Products.Application.Features.Commands.Update
{
    /// <summary>
    /// Handler cho command cập nhật sản phẩm
    /// </summary>
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, Product>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> HandleAsync(UpdateProductCommand command)
        {
            // Kiểm tra sản phẩm có tồn tại không
            var existingProduct = await _unitOfWork.GetRepository<Product>().GetByIdAsync(command.Id);
            if (existingProduct == null)
            {
                throw new ArgumentException($"Không tìm thấy sản phẩm với ID: {command.Id}");
            }

            // Validation
            if (string.IsNullOrWhiteSpace(command.ProductName))
            {
                throw new ArgumentException("Tên sản phẩm không được để trống");
            }

            if (string.IsNullOrWhiteSpace(command.ImgUrls))
            {
                throw new ArgumentException("Hình ảnh sản phẩm không được để trống");
            }

            // Cập nhật thông tin
            existingProduct.ProductName = command.ProductName;
            existingProduct.Description = command.Description;
            existingProduct.Status = command.Status;
            existingProduct.ImgUrls = command.ImgUrls;
            existingProduct.UpdatedAt = DateTimeOffset.UtcNow;

            // Lưu vào database
            await _unitOfWork.GetRepository<Product>().UpdateAsync(existingProduct);
            await _unitOfWork.SaveAsync();

            return existingProduct;
        }
    }
}
