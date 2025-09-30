using CareNest_Products.Application.Common;
using MediatR;

namespace CareNest_Products.Application.Features.Queries.GetAllPaging
{
    /// <summary>
    /// Query lấy danh sách danh mục sản phẩm có phân trang
    /// </summary>
    public class GetAllProductCategoriesPagingQuery : IRequest<PageResult<ProductCategoryResponse>>
    {
        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int Index { get; set; } = 1;

        /// <summary>
        /// Số lượng item per page
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Cột sắp xếp
        /// </summary>
        public string? SortColumn { get; set; }

        /// <summary>
        /// Hướng sắp xếp (asc/desc)
        /// </summary>
        public string? SortDirection { get; set; } = "asc";

        /// <summary>
        /// Lọc theo ProductId
        /// </summary>
        public string? ProductId { get; set; }

        /// <summary>
        /// Tìm kiếm theo tên danh mục
        /// </summary>
        public string? SearchTerm { get; set; }
    }
}
