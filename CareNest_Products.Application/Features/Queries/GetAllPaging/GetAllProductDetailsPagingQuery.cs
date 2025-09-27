using CareNest_Products.Application.Common;
using MediatR;

namespace CareNest_Products.Application.Features.Queries.GetAllPaging
{
    /// <summary>
    /// Query lấy danh sách chi tiết sản phẩm có phân trang
    /// </summary>
    public class GetAllProductDetailsPagingQuery : IRequest<PageResult<ProductDetailResponse>>
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
        /// Lọc theo CategoryId
        /// </summary>
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Tìm kiếm theo tên chi tiết
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Lọc theo trạng thái kho
        /// </summary>
        public bool? Status { get; set; }

        /// <summary>
        /// Lọc theo giá tối thiểu
        /// </summary>
        public int? MinPrice { get; set; }

        /// <summary>
        /// Lọc theo giá tối đa
        /// </summary>
        public int? MaxPrice { get; set; }
    }
}
