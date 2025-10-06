using CareNest_Products.Application.Common;
using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Domain.Entities;
using MediatR;

namespace CareNest_Products.Application.Features.Queries.GetAllPaging
{
    /// <summary>
    /// Handler cho query lấy danh sách sản phẩm có phân trang
    /// </summary>
    public class GetAllProductsPagingQueryHandler : IRequestHandler<GetAllProductsPagingQuery, PageResult<ProductResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllProductsPagingQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PageResult<ProductResponse>> Handle(GetAllProductsPagingQuery query, CancellationToken cancellationToken)
        {
            // Tạo predicate cho filtering
            var predicate = CreatePredicate(query);

            // Tạo order by function
            var orderByFunc = GetOrderByFunc(query.SortColumn, query.SortDirection);

            // Lấy dữ liệu với phân trang
            var result = await _unitOfWork.GetRepository<Domain.Entities.Product>().FindAsync(
                predicate: predicate,
                orderBy: orderByFunc,
                selector: p => new ProductResponse
                {
                    Id = p.Id,
                    ProductCategoryId = p.ProductCategoryId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Status = p.Status,
                    ImgUrls = p.ImgUrls,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    CreatedBy = p.CreatedBy,
                    UpdatedBy = p.UpdatedBy
                },
                pageSize: query.PageSize,
                pageIndex: query.Index);

            // Đếm tổng số records
            var totalCount = await _unitOfWork.GetRepository<Domain.Entities.Product>().CountAsync(predicate);

            return new PageResult<ProductResponse>(result, totalCount, query.PageSize, query.Index);
        }

        private System.Linq.Expressions.Expression<Func<Product, bool>>? CreatePredicate(GetAllProductsPagingQuery query)
        {
            return p =>
                (string.IsNullOrEmpty(query.SearchTerm) || p.ProductName.Contains(query.SearchTerm)) &&
                (string.IsNullOrEmpty(query.ProductCategoryId) || p.ProductCategoryId == query.ProductCategoryId) &&
                (!query.Status.HasValue || p.Status == query.Status.Value);
        }

        private Func<IQueryable<Product>, IOrderedQueryable<Product>> GetOrderByFunc(string? sortColumn, string? sortDirection)
        {
            var ascending = string.IsNullOrWhiteSpace(sortDirection) || sortDirection.ToLower() != "desc";

            return sortColumn?.ToLower() switch
            {
                "productname" => q => ascending ? q.OrderBy(p => p.ProductName) : q.OrderByDescending(p => p.ProductName),
                "createdat" => q => ascending ? q.OrderBy(p => p.CreatedAt) : q.OrderByDescending(p => p.CreatedAt),
                "updatedat" => q => ascending ? q.OrderBy(p => p.UpdatedAt) : q.OrderByDescending(p => p.UpdatedAt),
                "status" => q => ascending ? q.OrderBy(p => p.Status) : q.OrderByDescending(p => p.Status),
                _ => q => q.OrderBy(p => p.CreatedAt)
            };
        }
    }
}
