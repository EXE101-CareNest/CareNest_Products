using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Domain.Entities;

namespace CareNest_Products.Application.Features.Commands.Create
{
    /// <summary>
    /// Handler cho command tạo mới chi tiết sản phẩm
    /// </summary>
    public class CreateProductDetailCommandHandler : ICommandHandler<CreateProductDetailCommand, ProductDetail>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductDetailCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductDetail> HandleAsync(CreateProductDetailCommand command)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                throw new ArgumentException("Tên chi tiết không được để trống");
            }

            if (command.Price <= 0)
            {
                throw new ArgumentException("Giá sản phẩm phải lớn hơn 0");
            }

            if (command.QuantityInStock < 0)
            {
                throw new ArgumentException("Số lượng tồn kho phải >= 0");
            }

            if (command.Discount.HasValue && (command.Discount < 0 || command.Discount > 100))
            {
                throw new ArgumentException("Phần trăm giảm giá phải từ 0 đến 100");
            }

            if (string.IsNullOrWhiteSpace(command.ProductId))
            {
                throw new ArgumentException("ProductId không được để trống");
            }

            // Kiểm tra sản phẩm có tồn tại không
            var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(command.ProductId);
            if (product == null)
            {
                throw new ArgumentException($"Không tìm thấy sản phẩm với ID: {command.ProductId}");
            }

            // Tạo entity mới
            var detail = new ProductDetail
            {
                ProductId = command.ProductId,
                Name = command.Name,
                Price = command.Price,
                Status = command.Status,
                Discount = command.Discount,
                IsDefault = command.IsDefault,
                ImgUrls = command.ImgUrls,
                QuantityInStock = command.QuantityInStock,
                CreatedAt = DateTimeOffset.UtcNow
            };

            // Lưu vào database
            await _unitOfWork.GetRepository<ProductDetail>().AddAsync(detail);
            await _unitOfWork.SaveAsync();

            return detail;
        }
    }
}
