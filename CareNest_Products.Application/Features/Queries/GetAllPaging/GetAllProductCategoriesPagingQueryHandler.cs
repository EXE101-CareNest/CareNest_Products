using CareNest_Products.Application.Common;
using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Domain.Entities;
using MediatR;

namespace CareNest_Products.Application.Features.Queries.GetAllPaging
{
    /// <summary>
    /// Handler cho query lấy danh sách danh mục sản phẩm có phân trang
    /// </summary>
    public class GetAllProductCategoriesPagingQueryHandler : IRequestHandler<GetAllProductCategoriesPagingQuery, PageResult<ProductCategoryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllProductCategoriesPagingQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PageResult<ProductCategoryResponse>> Handle(GetAllProductCategoriesPagingQuery query, CancellationToken cancellationToken)
        {
            // Tạo predicate cho filtering
            var predicate = CreatePredicate(query);

            // Tạo order by function
            var orderByFunc = GetOrderByFunc(query.SortColumn, query.SortDirection);

            // Lấy dữ liệu với phân trang
            var result = await _unitOfWork.GetRepository<ProductCategory>().FindAsync(
                predicate: predicate,
                orderBy: orderByFunc,
                selector: pc => new ProductCategoryResponse
                {
                    Id = pc.Id,
                    ProductId = pc.ProductId,
                    Name = pc.Name,
                    CreatedAt = pc.CreatedAt,
                    UpdatedAt = pc.UpdatedAt,
                    CreatedBy = pc.CreatedBy,
                    UpdatedBy = pc.UpdatedBy
                },
                pageSize: query.PageSize,
                pageIndex: query.Index);

            // Đếm tổng số records
            var totalCount = await _unitOfWork.GetRepository<ProductCategory>().CountAsync(predicate);

            return new PageResult<ProductCategoryResponse>(result, totalCount, query.PageSize, query.Index);
        }

        private System.Linq.Expressions.Expression<Func<ProductCategory, bool>>? CreatePredicate(GetAllProductCategoriesPagingQuery query)
        {
            return pc =>
                (!query.ProductId.HasValue || pc.ProductId == query.ProductId.Value) &&
                (string.IsNullOrEmpty(query.SearchTerm) || pc.Name.Contains(query.SearchTerm));
        }

        private Func<IQueryable<ProductCategory>, IOrderedQueryable<ProductCategory>> GetOrderByFunc(string? sortColumn, string? sortDirection)
        {
            var ascending = string.IsNullOrWhiteSpace(sortDirection) || sortDirection.ToLower() != "desc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => ascending ? q.OrderBy(pc => pc.Name) : q.OrderByDescending(pc => pc.Name),
                "createdat" => q => ascending ? q.OrderBy(pc => pc.CreatedAt) : q.OrderByDescending(pc => pc.CreatedAt),
                "updatedat" => q => ascending ? q.OrderBy(pc => pc.UpdatedAt) : q.OrderByDescending(pc => pc.UpdatedAt),
                _ => q => q.OrderBy(pc => pc.CreatedAt)
            };
        }
    }
}
