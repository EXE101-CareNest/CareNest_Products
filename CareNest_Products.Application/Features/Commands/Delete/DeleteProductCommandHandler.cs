using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Domain.Entities;

namespace CareNest_Products.Application.Features.Commands.Delete
{
    /// <summary>
    /// Handler cho command xóa sản phẩm
    /// </summary>
    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(DeleteProductCommand command)
        {
            // Kiểm tra sản phẩm có tồn tại không
            var existingProduct = await _unitOfWork.GetRepository<Product>().GetByIdAsync(command.Id);
            if (existingProduct == null)
            {
                throw new ArgumentException($"Không tìm thấy sản phẩm với ID: {command.Id}");
            }

            // Xóa sản phẩm (cascade delete sẽ xóa các ProductCategory và ProductDetail liên quan)
            await _unitOfWork.GetRepository<Product>().DeleteAsync(existingProduct);
            await _unitOfWork.SaveAsync();
        }
    }
}
