using CareNest_Products.Application.Common;
using CareNest_Products.Application.Interfaces.CQRS.Queries;

namespace CareNest_Products.Application.Features.Queries.GetAllPaging
{
    /// <summary>
    /// Query lấy danh sách sản phẩm có phân trang
    /// </summary>
    public class GetAllProductsPagingQuery : IQuery<PageResult<ProductResponse>>
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
        /// Tìm kiếm theo tên sản phẩm
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Lọc theo ShopId
        /// </summary>
        public Guid? ShopId { get; set; }

        /// <summary>
        /// Lọc theo trạng thái
        /// </summary>
        public bool? Status { get; set; }
    }
}
