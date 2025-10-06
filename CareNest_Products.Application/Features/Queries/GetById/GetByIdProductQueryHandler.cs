using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Domain.Entities;
using MediatR;
using CareNest_Products.Application.Features.Queries.GetAllPaging;

namespace CareNest_Products.Application.Features.Queries.GetById
{
    /// <summary>
    /// Handler cho query lấy sản phẩm theo ID
    /// </summary>
    public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQuery, ProductWithCategoriesResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdProductQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductWithCategoriesResponse> Handle(GetByIdProductQuery query, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(query.Id);
            
            if (product == null)
            {
                throw new ArgumentException($"Không tìm thấy sản phẩm với ID: {query.Id}");
            }

            // Lấy danh mục thuộc sản phẩm bằng query repo
            var categoryRepo = _unitOfWork.GetRepository<ProductCategory>();
            var categories = await categoryRepo.FindAsync(pc => pc.Id == product.ProductCategoryId,
                orderBy: q => q.OrderBy(c => c.CreatedAt),
                selector: c => new ProductCategoryResponse
                {
                    Id = c.Id,
                    ShopId = c.ShopId,
                    Name = c.Name,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    CreatedBy = c.CreatedBy,
                    UpdatedBy = c.UpdatedBy
                });

            return new ProductWithCategoriesResponse
            {
                Id = product.Id,
                ProductCategoryId = product.ProductCategoryId,
                ProductName = product.ProductName,
                Description = product.Description,
                Status = product.Status,
                ImgUrls = product.ImgUrls,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                CreatedBy = product.CreatedBy,
                UpdatedBy = product.UpdatedBy,
                Categories = categories.ToList()
            };
        }
    }
}
