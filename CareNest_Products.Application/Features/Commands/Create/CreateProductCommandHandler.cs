using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Application.Interfaces.Services;
using CareNest_Products.Domain.Entities;

namespace CareNest_Products.Application.Features.Commands.Create
{
    /// <summary>
    /// Handler cho command tạo mới sản phẩm
    /// </summary>
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Product>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAPIService _apiService;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IAPIService apiService)
        {
            _unitOfWork = unitOfWork;
            _apiService = apiService;
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
            if (string.IsNullOrWhiteSpace(command.ProductCategoryId))
            {
                throw new ArgumentException("ProductCategoryId không được để trống");
            }

            // Kiểm tra danh mục sản phẩm có tồn tại không
            var category = await _unitOfWork.GetRepository<ProductCategory>().GetByIdAsync(command.ProductCategoryId);
            if (category == null)
            {
                throw new ArgumentException($"Không tìm thấy danh mục sản phẩm với ID: {command.ProductCategoryId}");
            }

            // Tạo entity mới
            var product = new Product
            {
                ProductCategoryId = command.ProductCategoryId,
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
