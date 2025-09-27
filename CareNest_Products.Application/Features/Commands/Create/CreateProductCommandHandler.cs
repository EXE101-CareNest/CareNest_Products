using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Domain.Entities;

namespace CareNest_Products.Application.Features.Commands.Create
{
    /// <summary>
    /// Handler cho command tạo mới sản phẩm
    /// </summary>
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Product>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> HandleAsync(CreateProductCommand command)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(command.ProductName))
            {
                throw new ArgumentException("Tên sản phẩm không được để trống");
            }

            if (string.IsNullOrWhiteSpace(command.ImgUrls))
            {
                throw new ArgumentException("Hình ảnh sản phẩm không được để trống");
            }

            // Tạo entity mới
            var product = new Product
            {
                ShopId = command.ShopId,
                ProductName = command.ProductName,
                Description = command.Description,
                Status = command.Status,
                ImgUrls = command.ImgUrls,
                CreatedAt = DateTimeOffset.UtcNow
            };

            // Lưu vào database
            await _unitOfWork.GetRepository<Domain.Entities.Product>().AddAsync(product);
            await _unitOfWork.SaveAsync();

            return product;
        }
    }
}
