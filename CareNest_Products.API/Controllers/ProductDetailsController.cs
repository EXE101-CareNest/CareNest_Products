using CareNest_Products.Application.Common;
using CareNest_Products.Application.Features.Commands.Create;
using CareNest_Products.Application.Features.Queries.GetAllPaging;
using CareNest_Products.Application.Interfaces.CQRS;
using CareNest_Products.API.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CareNest_Products.API.Controllers
{
    /// <summary>
    /// Controller quản lý chi tiết sản phẩm
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailsController : ControllerBase
    {
        private readonly IUseCaseDispatcher _dispatcher;

        public ProductDetailsController(IUseCaseDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Lấy danh sách chi tiết sản phẩm với phân trang
        /// </summary>
        /// <param name="pageIndex">Trang hiện tại (mặc định: 1)</param>
        /// <param name="pageSize">Số lượng item per page (mặc định: 10)</param>
        /// <param name="sortColumn">Cột sắp xếp</param>
        /// <param name="sortDirection">Hướng sắp xếp (asc/desc)</param>
        /// <param name="categoryId">Lọc theo CategoryId</param>
        /// <param name="searchTerm">Tìm kiếm theo tên chi tiết</param>
        /// <param name="status">Lọc theo trạng thái kho</param>
        /// <param name="minPrice">Lọc theo giá tối thiểu</param>
        /// <param name="maxPrice">Lọc theo giá tối đa</param>
        /// <returns>Danh sách chi tiết sản phẩm có phân trang</returns>
        [HttpGet]
        public async Task<IActionResult> GetPaging(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortColumn = null,
            [FromQuery] string? sortDirection = "asc",
            [FromQuery] Guid? categoryId = null,
            [FromQuery] string? searchTerm = null,
            [FromQuery] bool? status = null,
            [FromQuery] int? minPrice = null,
            [FromQuery] int? maxPrice = null)
        {
            try
            {
                var query = new GetAllProductDetailsPagingQuery
                {
                    Index = pageIndex,
                    PageSize = pageSize,
                    SortColumn = sortColumn,
                    SortDirection = sortDirection,
                    CategoryId = categoryId,
                    SearchTerm = searchTerm,
                    Status = status,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice
                };

                var result = await _dispatcher.DispatchQueryAsync<GetAllProductDetailsPagingQuery, PageResult<ProductDetailResponse>>(query);
                return this.OkResponse(result, "Lấy danh sách chi tiết sản phẩm thành công");
            }
            catch (Exception ex)
            {
                return this.ErrorResponse($"Lỗi khi lấy danh sách chi tiết sản phẩm: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// Lấy chi tiết theo danh mục
        /// </summary>
        /// <param name="categoryId">ID của danh mục</param>
        /// <returns>Danh sách chi tiết của danh mục</returns>
        [HttpGet("categories/{categoryId}")]
        public async Task<IActionResult> GetByCategoryId(Guid categoryId)
        {
            try
            {
                var query = new GetAllProductDetailsPagingQuery
                {
                    CategoryId = categoryId,
                    PageSize = 1000 // Lấy tất cả chi tiết của danh mục
                };

                var result = await _dispatcher.DispatchQueryAsync<GetAllProductDetailsPagingQuery, PageResult<ProductDetailResponse>>(query);
                return this.OkResponse(result.Data, "Lấy chi tiết danh mục thành công");
            }
            catch (Exception ex)
            {
                return this.ErrorResponse($"Lỗi khi lấy chi tiết danh mục: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// Tạo chi tiết mới cho danh mục
        /// </summary>
        /// <param name="categoryId">ID của danh mục</param>
        /// <param name="command">Thông tin chi tiết mới</param>
        /// <returns>Chi tiết vừa tạo</returns>
        [HttpPost("categories/{categoryId}")]
        public async Task<IActionResult> Create(Guid categoryId, [FromBody] CreateProductDetailCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return this.ErrorResponse("Dữ liệu không hợp lệ", 400, errors);
                }

                command.CategoryId = categoryId;
                var result = await _dispatcher.DispatchAsync<CreateProductDetailCommand, Domain.Entities.ProductDetail>(command);
                return this.OkResponse(result, "Tạo chi tiết sản phẩm thành công");
            }
            catch (ArgumentException ex)
            {
                return this.ErrorResponse(ex.Message, 400);
            }
            catch (Exception ex)
            {
                return this.ErrorResponse($"Lỗi khi tạo chi tiết sản phẩm: {ex.Message}", 500);
            }
        }
    }
}
