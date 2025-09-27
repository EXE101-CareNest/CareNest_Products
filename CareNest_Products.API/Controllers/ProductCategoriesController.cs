using CareNest_Products.Application.Common;
using CareNest_Products.Application.Features.Commands.Create;
using CareNest_Products.Application.Features.Queries.GetAllPaging;
using CareNest_Products.Application.Interfaces.CQRS;
using CareNest_Products.API.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CareNest_Products.API.Controllers
{
    /// <summary>
    /// Controller quản lý danh mục sản phẩm
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IUseCaseDispatcher _dispatcher;

        public ProductCategoriesController(IUseCaseDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Lấy danh sách danh mục sản phẩm với phân trang
        /// </summary>
        /// <param name="pageIndex">Trang hiện tại (mặc định: 1)</param>
        /// <param name="pageSize">Số lượng item per page (mặc định: 10)</param>
        /// <param name="sortColumn">Cột sắp xếp</param>
        /// <param name="sortDirection">Hướng sắp xếp (asc/desc)</param>
        /// <param name="productId">Lọc theo ProductId</param>
        /// <param name="searchTerm">Tìm kiếm theo tên danh mục</param>
        /// <returns>Danh sách danh mục sản phẩm có phân trang</returns>
        [HttpGet]
        public async Task<IActionResult> GetPaging(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortColumn = null,
            [FromQuery] string? sortDirection = "asc",
            [FromQuery] Guid? productId = null,
            [FromQuery] string? searchTerm = null)
        {
            try
            {
                var query = new GetAllProductCategoriesPagingQuery
                {
                    Index = pageIndex,
                    PageSize = pageSize,
                    SortColumn = sortColumn,
                    SortDirection = sortDirection,
                    ProductId = productId,
                    SearchTerm = searchTerm
                };

                var result = await _dispatcher.DispatchQueryAsync<GetAllProductCategoriesPagingQuery, PageResult<ProductCategoryResponse>>(query);
                return this.OkResponse(result, "Lấy danh sách danh mục sản phẩm thành công");
            }
            catch (Exception ex)
            {
                return this.ErrorResponse($"Lỗi khi lấy danh sách danh mục sản phẩm: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// Lấy danh mục của sản phẩm
        /// </summary>
        /// <param name="productId">ID của sản phẩm</param>
        /// <returns>Danh sách danh mục của sản phẩm</returns>
        [HttpGet("products/{productId}")]
        public async Task<IActionResult> GetByProductId(Guid productId)
        {
            try
            {
                var query = new GetAllProductCategoriesPagingQuery
                {
                    ProductId = productId,
                    PageSize = 1000 // Lấy tất cả danh mục của sản phẩm
                };

                var result = await _dispatcher.DispatchQueryAsync<GetAllProductCategoriesPagingQuery, PageResult<ProductCategoryResponse>>(query);
                return this.OkResponse(result.Data, "Lấy danh mục sản phẩm thành công");
            }
            catch (Exception ex)
            {
                return this.ErrorResponse($"Lỗi khi lấy danh mục sản phẩm: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// Tạo danh mục mới cho sản phẩm
        /// </summary>
        /// <param name="productId">ID của sản phẩm</param>
        /// <param name="command">Thông tin danh mục mới</param>
        /// <returns>Danh mục vừa tạo</returns>
        [HttpPost("products/{productId}")]
        public async Task<IActionResult> Create(Guid productId, [FromBody] CreateProductCategoryCommand command)
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

                command.ProductId = productId;
                var result = await _dispatcher.DispatchAsync<CreateProductCategoryCommand, Domain.Entities.ProductCategory>(command);
                return this.OkResponse(result, "Tạo danh mục sản phẩm thành công");
            }
            catch (ArgumentException ex)
            {
                return this.ErrorResponse(ex.Message, 400);
            }
            catch (Exception ex)
            {
                return this.ErrorResponse($"Lỗi khi tạo danh mục sản phẩm: {ex.Message}", 500);
            }
        }
    }
}
