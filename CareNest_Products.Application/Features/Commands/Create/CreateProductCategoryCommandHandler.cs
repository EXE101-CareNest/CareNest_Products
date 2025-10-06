using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Domain.Entities;

namespace CareNest_Products.Application.Features.Commands.Create
{
    /// <summary>
    /// Handler cho command tạo mới danh mục sản phẩm
    /// </summary>
    public class CreateProductCategoryCommandHandler : ICommandHandler<CreateProductCategoryCommand, ProductCategory>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductCategory> HandleAsync(CreateProductCategoryCommand command)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                throw new ArgumentException("Tên danh mục không được để trống");
            }

            if (string.IsNullOrWhiteSpace(command.ShopId))
            {
                throw new ArgumentException("ShopId không được để trống");
            }

            // Tạo entity mới
            var category = new ProductCategory
            {
                ShopId = command.ShopId,
                Name = command.Name,
                CreatedAt = DateTimeOffset.UtcNow
            };

            // Lưu vào database
            await _unitOfWork.GetRepository<ProductCategory>().AddAsync(category);
            await _unitOfWork.SaveAsync();

            return category;
        }
    }
}
