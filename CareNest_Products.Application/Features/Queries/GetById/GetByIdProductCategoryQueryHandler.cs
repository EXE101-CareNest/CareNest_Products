using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Domain.Entities;
using MediatR;
using CareNest_Products.Application.Features.Queries.GetAllPaging;

namespace CareNest_Products.Application.Features.Queries.GetById
{
    /// <summary>
    /// Handler cho query lấy danh mục sản phẩm theo ID
    /// </summary>
    public class GetByIdProductCategoryQueryHandler : IRequestHandler<GetByIdProductCategoryQuery, ProductCategoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdProductCategoryQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductCategoryResponse> Handle(GetByIdProductCategoryQuery query, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.GetRepository<ProductCategory>().GetByIdAsync(query.Id);
            
            if (category == null)
            {
                throw new ArgumentException($"Không tìm thấy danh mục sản phẩm với ID: {query.Id}");
            }

            return new ProductCategoryResponse
            {
                Id = category.Id,
                ProductId = category.ProductId,
                Name = category.Name,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt,
                CreatedBy = category.CreatedBy,
                UpdatedBy = category.UpdatedBy
            };
        }
    }
}
