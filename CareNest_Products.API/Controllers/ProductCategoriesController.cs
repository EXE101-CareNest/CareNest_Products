using CareNest_Products.Application.Common;
using CareNest_Products.Application.Features.Commands.Create;
using CareNest_Products.Application.Features.Commands.Update;
using CareNest_Products.Application.Features.Commands.Delete;
using CareNest_Products.Application.Features.Queries.GetAllPaging;
using CareNest_Products.Application.Interfaces.CQRS;
using CareNest_Products.Application.Features.Queries.GetAllPaging;
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
        /// <param name="productId">Lọc theo ProductId</param>
        /// <param name="searchTerm">Tìm kiếm theo tên danh mục</param>
        /// <returns>Danh sách danh mục sản phẩm có phân trang</returns>
        [HttpGet]
        public async Task<IActionResult> GetPaging(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortColumn = null,
            [FromQuery] string? sortDirection = "asc",
            [FromQuery] string? productId = null,
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

                var result = await _mediator.Send(query);
                return this.OkResponse(result, "Lấy danh sách danh mục sản phẩm thành công");
            }
            catch (Exception ex)
            {
                return this.ErrorResponse<object>($"Lỗi khi lấy danh sách danh mục sản phẩm: {ex.Message}");
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
                    ProductId = updated.ProductId,
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
        /// Lấy danh mục của sản phẩm
        /// </summary>
        /// <param name="productId">ID của sản phẩm</param>
        /// <returns>Danh sách danh mục của sản phẩm</returns>
        [HttpGet("products/{productId}")]
        public async Task<IActionResult> GetByProductId(string productId)
        {
            try
            {
                var query = new GetAllProductCategoriesPagingQuery
                {
                    ProductId = productId,
                    PageSize = 1000 // Lấy tất cả danh mục của sản phẩm
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
        /// Tạo danh mục mới cho sản phẩm
        /// </summary>
        /// <param name="productId">ID của sản phẩm</param>
        /// <param name="command">Thông tin danh mục mới</param>
        /// <returns>Danh mục vừa tạo</returns>
        [HttpPost("products/{productId}")]
        public async Task<IActionResult> Create(string productId, [FromBody] CreateProductCategoryCommand command)
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

                command.ProductId = productId;
                var created = await _dispatcher.DispatchAsync<CreateProductCategoryCommand, CareNest_Products.Domain.Entities.ProductCategory>(command);
                var dto = new ProductCategoryResponse
                {
                    Id = created.Id,
                    ProductId = created.ProductId,
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
