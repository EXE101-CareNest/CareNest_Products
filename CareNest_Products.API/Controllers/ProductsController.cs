using CareNest_Products.Application.Common;
using CareNest_Products.Application.Features.Commands.Create;
using CareNest_Products.Application.Features.Commands.Update;
using CareNest_Products.Application.Features.Commands.Delete;
using CareNest_Products.Application.Features.Queries.GetAllPaging;
using CareNest_Products.Application.Features.Queries.GetById;
using CareNest_Products.Application.Interfaces.CQRS;
using CareNest_Products.API.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CareNest_Products.API.Controllers
{
    /// <summary>
    /// Controller quản lý sản phẩm
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUseCaseDispatcher _dispatcher;

        public ProductsController(IUseCaseDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Lấy danh sách sản phẩm với phân trang
        /// </summary>
        /// <param name="pageIndex">Trang hiện tại (mặc định: 1)</param>
        /// <param name="pageSize">Số lượng item per page (mặc định: 10)</param>
        /// <param name="sortColumn">Cột sắp xếp</param>
        /// <param name="sortDirection">Hướng sắp xếp (asc/desc)</param>
        /// <param name="searchTerm">Tìm kiếm theo tên sản phẩm</param>
        /// <param name="shopId">Lọc theo ShopId</param>
        /// <param name="status">Lọc theo trạng thái</param>
        /// <returns>Danh sách sản phẩm có phân trang</returns>
        [HttpGet]
        public async Task<IActionResult> GetPaging(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortColumn = null,
            [FromQuery] string? sortDirection = "asc",
            [FromQuery] string? searchTerm = null,
            [FromQuery] Guid? shopId = null,
            [FromQuery] bool? status = null)
        {
            try
            {
                var query = new GetAllProductsPagingQuery
                {
                    Index = pageIndex,
                    PageSize = pageSize,
                    SortColumn = sortColumn,
                    SortDirection = sortDirection,
                    SearchTerm = searchTerm,
                    ShopId = shopId,
                    Status = status
                };

                var result = await _dispatcher.DispatchQueryAsync<GetAllProductsPagingQuery, PageResult<ProductResponse>>(query);
                return this.OkResponse(result, "Lấy danh sách sản phẩm thành công");
            }
            catch (Exception ex)
            {
                return this.ErrorResponse($"Lỗi khi lấy danh sách sản phẩm: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// Lấy thông tin sản phẩm theo ID
        /// </summary>
        /// <param name="id">ID của sản phẩm</param>
        /// <returns>Thông tin sản phẩm</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var query = new GetByIdProductQuery { Id = id };
                var result = await _dispatcher.DispatchQueryAsync<GetByIdProductQuery, Domain.Entities.Product>(query);
                return this.OkResponse(result, "Lấy thông tin sản phẩm thành công");
            }
            catch (ArgumentException ex)
            {
                return this.ErrorResponse(ex.Message, 404);
            }
            catch (Exception ex)
            {
                return this.ErrorResponse($"Lỗi khi lấy thông tin sản phẩm: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// Tạo mới sản phẩm
        /// </summary>
        /// <param name="command">Thông tin sản phẩm mới</param>
        /// <returns>Sản phẩm vừa tạo</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
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

                var result = await _dispatcher.DispatchAsync<CreateProductCommand, Domain.Entities.Product>(command);
                return this.OkResponse(result, "Tạo sản phẩm thành công");
            }
            catch (ArgumentException ex)
            {
                return this.ErrorResponse(ex.Message, 400);
            }
            catch (Exception ex)
            {
                return this.ErrorResponse($"Lỗi khi tạo sản phẩm: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// Cập nhật sản phẩm
        /// </summary>
        /// <param name="id">ID của sản phẩm cần cập nhật</param>
        /// <param name="command">Thông tin sản phẩm mới</param>
        /// <returns>Sản phẩm đã cập nhật</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command)
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

                command.Id = id;
                var result = await _dispatcher.DispatchAsync<UpdateProductCommand, Domain.Entities.Product>(command);
                return this.OkResponse(result, "Cập nhật sản phẩm thành công");
            }
            catch (ArgumentException ex)
            {
                return this.ErrorResponse(ex.Message, 404);
            }
            catch (Exception ex)
            {
                return this.ErrorResponse($"Lỗi khi cập nhật sản phẩm: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// Xóa sản phẩm
        /// </summary>
        /// <param name="id">ID của sản phẩm cần xóa</param>
        /// <returns>Kết quả xóa</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var command = new DeleteProductCommand { Id = id };
                await _dispatcher.DispatchAsync(command);
                return this.OkResponse("Xóa sản phẩm thành công");
            }
            catch (ArgumentException ex)
            {
                return this.ErrorResponse(ex.Message, 404);
            }
            catch (Exception ex)
            {
                return this.ErrorResponse($"Lỗi khi xóa sản phẩm: {ex.Message}", 500);
            }
        }
    }
}
