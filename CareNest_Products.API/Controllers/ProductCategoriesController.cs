using CareNest_Products.Application.Common;
using CareNest_Products.Application.Features.Commands.Create;
using CareNest_Products.Application.Features.Commands.Update;
using CareNest_Products.Application.Features.Commands.Delete;
using CareNest_Products.Application.Features.Queries.GetAllPaging;
using CareNest_Products.Application.Features.Queries.GetById;
using CareNest_Products.Application.Interfaces.CQRS;
using CareNest_Products.API.Extensions;
using MediatR;
using CareNest_Products.Application.Interfaces.CQRS;
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
        private readonly IMediator _mediator;
        private readonly IUseCaseDispatcher _dispatcher;

        public ProductCategoriesController(IMediator mediator, IUseCaseDispatcher dispatcher)
        {
            _mediator = mediator;
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Lấy danh sách danh mục sản phẩm với phân trang
        /// </summary>
        /// <param name="pageIndex">Trang hiện tại (mặc định: 1)</param>
        /// <param name="pageSize">Số lượng item per page (mặc định: 10)</param>
        /// <param name="sortColumn">Cột sắp xếp</param>
        /// <param name="sortDirection">Hướng sắp xếp (asc/desc)</param>
        /// <param name="shopId">Lọc theo ShopId</param>
        /// <param name="searchTerm">Tìm kiếm theo tên danh mục</param>
        /// <returns>Danh sách danh mục sản phẩm có phân trang</returns>
        [HttpGet]
        public async Task<IActionResult> GetPaging(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortColumn = null,
            [FromQuery] string? sortDirection = "asc",
            [FromQuery] string? shopId = null,
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
                    ShopId = shopId,
                    SearchTerm = searchTerm
                };

                var result = await _mediator.Send(query);
                return this.OkResponse(result, "Lấy danh sách danh mục sản phẩm thành công");
            }
            catch (Exception ex)
            {
                return this.ErrorResponse<object>($"Lỗi khi lấy danh sách danh mục sản phẩm: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy danh mục sản phẩm theo ID
        /// </summary>
        /// <param name="id">ID danh mục</param>
        /// <returns>Thông tin danh mục sản phẩm</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var query = new GetByIdProductCategoryQuery { Id = id };
                var result = await _mediator.Send(query);
                return this.OkResponse(result, "Lấy danh mục sản phẩm thành công");
            }
            catch (ArgumentException ex)
            {
                return this.ErrorResponse<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return this.ErrorResponse<object>($"Lỗi khi lấy danh mục sản phẩm: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật danh mục sản phẩm
        /// </summary>
        /// <param name="id">ID danh mục</param>
        /// <param name="command">Thông tin cập nhật</param>
        /// <returns>Danh mục đã cập nhật</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateProductCategoryCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return this.ErrorResponse<object>("Dữ liệu không hợp lệ");
                }

                command.Id = id;
                var updated = await _dispatcher.DispatchAsync<UpdateProductCategoryCommand, CareNest_Products.Domain.Entities.ProductCategory>(command);
                var dto = new ProductCategoryResponse
                {
                    Id = updated.Id,
                    ShopId = updated.ShopId,
                    Name = updated.Name,
                    CreatedAt = updated.CreatedAt,
                    UpdatedAt = updated.UpdatedAt,
                    CreatedBy = updated.CreatedBy,
                    UpdatedBy = updated.UpdatedBy
                };
                return this.OkResponse(dto, "Cập nhật danh mục sản phẩm thành công");
            }
            catch (ArgumentException ex)
            {
                return this.ErrorResponse<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return this.ErrorResponse<object>($"Lỗi khi cập nhật danh mục sản phẩm: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa danh mục sản phẩm
        /// </summary>
        /// <param name="id">ID danh mục</param>
        /// <returns>Kết quả xóa</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var command = new DeleteProductCategoryCommand { Id = id };
                await _dispatcher.DispatchAsync(command);
                return this.OkResponse("Xóa danh mục sản phẩm thành công");
            }
            catch (ArgumentException ex)
            {
                return this.ErrorResponse<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return this.ErrorResponse<object>($"Lỗi khi xóa danh mục sản phẩm: {ex.Message}");
            }
        }
        /// <summary>
        /// Lấy danh mục của shop
        /// </summary>
        /// <param name="shopId">ID của shop</param>
        /// <returns>Danh sách danh mục của shop</returns>
        [HttpGet("shops/{shopId}")]
        public async Task<IActionResult> GetByShopId(string shopId)
        {
            try
            {
                var query = new GetAllProductCategoriesPagingQuery
                {
                    ShopId = shopId,
                    PageSize = 1000 // Lấy tất cả danh mục của shop
                };

                var result = await _mediator.Send(query);
                return this.OkResponse(result, "Lấy danh mục sản phẩm thành công");
            }
            catch (Exception ex)
            {
                return this.ErrorResponse<object>($"Lỗi khi lấy danh mục sản phẩm: {ex.Message}");
            }
        }

        /// <summary>
        /// Tạo danh mục mới cho shop
        /// </summary>
        /// <param name="shopId">ID của shop</param>
        /// <param name="request">Thông tin danh mục mới</param>
        /// <returns>Danh mục vừa tạo</returns>
        [HttpPost("shops/{shopId}")]
        public async Task<IActionResult> Create(string shopId, [FromBody] CreateProductCategoryRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return this.ErrorResponse<object>("Dữ liệu không hợp lệ");
                }

                var command = new CreateProductCategoryCommand
                {
                    ShopId = shopId,
                    Name = request.Name
                };
                var created = await _dispatcher.DispatchAsync<CreateProductCategoryCommand, CareNest_Products.Domain.Entities.ProductCategory>(command);
                var dto = new ProductCategoryResponse
                {
                    Id = created.Id,
                    ShopId = created.ShopId,
                    Name = created.Name,
                    CreatedAt = created.CreatedAt,
                    UpdatedAt = created.UpdatedAt,
                    CreatedBy = created.CreatedBy,
                    UpdatedBy = created.UpdatedBy
                };
                return this.OkResponse(dto, "Tạo danh mục sản phẩm thành công");
            }
            catch (ArgumentException ex)
            {
                return this.ErrorResponse<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return this.ErrorResponse<object>($"Lỗi khi tạo danh mục sản phẩm: {ex.Message}");
            }
        }
    }
}
