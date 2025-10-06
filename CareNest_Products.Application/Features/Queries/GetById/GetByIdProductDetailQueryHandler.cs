using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Domain.Entities;
using MediatR;
using CareNest_Products.Application.Features.Queries.GetAllPaging;

namespace CareNest_Products.Application.Features.Queries.GetById
{
    /// <summary>
    /// Handler cho query lấy chi tiết sản phẩm theo ID
    /// </summary>
    public class GetByIdProductDetailQueryHandler : IRequestHandler<GetByIdProductDetailQuery, ProductDetailResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdProductDetailQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductDetailResponse> Handle(GetByIdProductDetailQuery query, CancellationToken cancellationToken)
        {
            var productDetail = await _unitOfWork.GetRepository<ProductDetail>().GetByIdAsync(query.Id);
            
            if (productDetail == null)
            {
                throw new ArgumentException($"Không tìm thấy chi tiết sản phẩm với ID: {query.Id}");
            }

            return new ProductDetailResponse
            {
                Id = productDetail.Id,
                ProductId = productDetail.ProductId,
                Name = productDetail.Name,
                Price = productDetail.Price,
                Status = productDetail.Status,
                Discount = productDetail.Discount,
                IsDefault = productDetail.IsDefault,
                ImgUrls = productDetail.ImgUrls,
                QuantityInStock = productDetail.QuantityInStock,
                CreatedAt = productDetail.CreatedAt,
                UpdatedAt = productDetail.UpdatedAt,
                CreatedBy = productDetail.CreatedBy,
                UpdatedBy = productDetail.UpdatedBy
            };
        }
    }
}
