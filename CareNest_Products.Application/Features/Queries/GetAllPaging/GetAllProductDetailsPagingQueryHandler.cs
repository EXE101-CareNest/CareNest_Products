using CareNest_Products.Application.Common;
using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Domain.Entities;
using MediatR;

namespace CareNest_Products.Application.Features.Queries.GetAllPaging
{
    /// <summary>
    /// Handler cho query lấy danh sách chi tiết sản phẩm có phân trang
    /// </summary>
    public class GetAllProductDetailsPagingQueryHandler : IRequestHandler<GetAllProductDetailsPagingQuery, PageResult<ProductDetailResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllProductDetailsPagingQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PageResult<ProductDetailResponse>> Handle(GetAllProductDetailsPagingQuery query, CancellationToken cancellationToken)
        {
            // Tạo predicate cho filtering
            var predicate = CreatePredicate(query);

            // Tạo order by function
            var orderByFunc = GetOrderByFunc(query.SortColumn, query.SortDirection);

            // Lấy dữ liệu với phân trang
            var result = await _unitOfWork.GetRepository<ProductDetail>().FindAsync(
                predicate: predicate,
                orderBy: orderByFunc,
                selector: pd => new ProductDetailResponse
                {
                    Id = pd.Id,
                    CategoryId = pd.CategoryId,
                    Name = pd.Name,
                    Price = pd.Price,
                    Status = pd.Status,
                    Discount = pd.Discount,
                    IsDefault = pd.IsDefault,
                    ImgUrls = pd.ImgUrls,
                    QuantityInStock = pd.QuantityInStock,
                    CreatedAt = pd.CreatedAt,
                    UpdatedAt = pd.UpdatedAt,
                    CreatedBy = pd.CreatedBy,
                    UpdatedBy = pd.UpdatedBy
                },
                pageSize: query.PageSize,
                pageIndex: query.Index);

            // Đếm tổng số records
            var totalCount = await _unitOfWork.GetRepository<ProductDetail>().CountAsync(predicate);

            return new PageResult<ProductDetailResponse>(result, totalCount, query.PageSize, query.Index);
        }

        private System.Linq.Expressions.Expression<Func<ProductDetail, bool>>? CreatePredicate(GetAllProductDetailsPagingQuery query)
        {
            return pd =>
                (!query.CategoryId.HasValue || pd.CategoryId == query.CategoryId.Value) &&
                (string.IsNullOrEmpty(query.SearchTerm) || pd.Name.Contains(query.SearchTerm)) &&
                (!query.Status.HasValue || pd.Status == query.Status.Value) &&
                (!query.MinPrice.HasValue || pd.Price >= query.MinPrice.Value) &&
                (!query.MaxPrice.HasValue || pd.Price <= query.MaxPrice.Value);
        }

        private Func<IQueryable<ProductDetail>, IOrderedQueryable<ProductDetail>> GetOrderByFunc(string? sortColumn, string? sortDirection)
        {
            var ascending = string.IsNullOrWhiteSpace(sortDirection) || sortDirection.ToLower() != "desc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => ascending ? q.OrderBy(pd => pd.Name) : q.OrderByDescending(pd => pd.Name),
                "price" => q => ascending ? q.OrderBy(pd => pd.Price) : q.OrderByDescending(pd => pd.Price),
                "status" => q => ascending ? q.OrderBy(pd => pd.Status) : q.OrderByDescending(pd => pd.Status),
                "quantityinstock" => q => ascending ? q.OrderBy(pd => pd.QuantityInStock) : q.OrderByDescending(pd => pd.QuantityInStock),
                "createdat" => q => ascending ? q.OrderBy(pd => pd.CreatedAt) : q.OrderByDescending(pd => pd.CreatedAt),
                "updatedat" => q => ascending ? q.OrderBy(pd => pd.UpdatedAt) : q.OrderByDescending(pd => pd.UpdatedAt),
                _ => q => q.OrderBy(pd => pd.CreatedAt)
            };
        }
    }
}
