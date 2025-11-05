using CareNest_Products.Application.Common;
using CareNest_Products.Application.Features.Commands.Create;
using CareNest_Products.Application.Features.Commands.Update;
using CareNest_Products.Application.Features.Commands.Delete;
using CareNest_Products.Application.Features.Queries.GetAllPaging;
using CareNest_Products.Application.Features.Queries.GetById;
using CareNest_Products.Application.Interfaces.CQRS;
using CareNest_Products.API.Extensions;
using CareNest_Products.API.Controllers.Models;
using CareNest_Products.Application.Interfaces.Services;
using MediatR;
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
        private readonly IMediator _mediator;
        private readonly IUseCaseDispatcher _dispatcher;
        private readonly IImageService _imageService;

        public ProductsController(IMediator mediator, IUseCaseDispatcher dispatcher, IImageService imageService)
        {
            _mediator = mediator;
            _dispatcher = dispatcher;
            _imageService = imageService;
        }

        /// <summary>
        /// Lấy danh sách sản phẩm với phân trang
        /// </summary>
        /// <param name="pageIndex">Trang hiện tại (mặc định: 1)</param>
        /// <param name="pageSize">Số lượng item per page (mặc định: 10)</param>
        /// <param name="sortColumn">Cột sắp xếp</param>
        /// <param name="sortDirection">Hướng sắp xếp (asc/desc)</param>
        /// <param name="searchTerm">Tìm kiếm theo tên sản phẩm</param>
        /// <param name="productCategoryId">Lọc theo ProductCategoryId</param>
        /// <param name="status">Lọc theo trạng thái</param>
        /// <param name="shopId">Lọc theo ShopId (qua danh mục)</param>
        /// <returns>Danh sách sản phẩm có phân trang</returns>
        [HttpGet]
        public async Task<IActionResult> GetPaging(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortColumn = null,
            [FromQuery] string? sortDirection = "asc",
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? productCategoryId = null,
            [FromQuery] bool? status = null,
            [FromQuery] string? shopId = null)
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
                    ProductCategoryId = productCategoryId,
                    Status = status,
                    ShopId = shopId
                };

                var result = await _mediator.Send(query);
                return this.OkResponse(result, "Lấy danh sách sản phẩm thành công");
            }
            catch (Exception ex)
            {
                return this.ErrorResponse<object>($"Lỗi khi lấy danh sách sản phẩm: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy thông tin sản phẩm theo ID
        /// </summary>
        /// <param name="id">ID của sản phẩm</param>
        /// <returns>Thông tin sản phẩm</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var query = new GetByIdProductQuery { Id = id };
                var result = await _mediator.Send(query);
                return this.OkResponse(result, "Lấy thông tin sản phẩm thành công");
            }
            catch (ArgumentException ex)
            {
                return this.ErrorResponse<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return this.ErrorResponse<object>($"Lỗi khi lấy thông tin sản phẩm: {ex.Message}");
            }
        }

        /// <summary>
        /// Tạo mới sản phẩm
        /// </summary>
        /// <param name="model">Form tạo sản phẩm (multipart/form-data)</param>
        /// <returns>Sản phẩm vừa tạo</returns>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateProductFormModel model)
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

                var command = new CreateProductCommand
                {
                    ProductCategoryId = model.ProductCategoryId,
                    ProductName = model.ProductName,
                    Description = model.Description,
                    Status = model.Status,
                    ImgUrls = string.Empty
                };

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    await using var stream = model.ImageFile.OpenReadStream();
                    var ownerId = "product";
                    var folder = model.ProductCategoryId;
                    var publicId = $"{model.ProductCategoryId}/{Guid.NewGuid()}";
                    var uploaded = await _imageService.UploadAsync(stream, model.ImageFile.FileName, model.ImageFile.ContentType, ownerId, folder, publicId);
                    command.ImgUrls = uploaded.OptimizedUrl ?? uploaded.SecureUrl;
                }

                var result = await _dispatcher.DispatchAsync<CreateProductCommand, CareNest_Products.Domain.Entities.Product>(command);
                return this.OkResponse(result, "Tạo sản phẩm thành công");
            }
            catch (ArgumentException ex)
            {
                return this.ErrorResponse<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return this.ErrorResponse<object>($"Lỗi khi tạo sản phẩm: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật sản phẩm
        /// </summary>
        /// <param name="id">ID của sản phẩm cần cập nhật</param>
        /// <param name="command">Thông tin sản phẩm mới</param>
        /// <returns>Sản phẩm đã cập nhật</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateProductCommand command)
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
                var result = await _dispatcher.DispatchAsync<UpdateProductCommand, CareNest_Products.Domain.Entities.Product>(command);
                return this.OkResponse(result, "Cập nhật sản phẩm thành công");
            }
            catch (ArgumentException ex)
            {
                return this.ErrorResponse<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return this.ErrorResponse<object>($"Lỗi khi cập nhật sản phẩm: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa sản phẩm
        /// </summary>
        /// <param name="id">ID của sản phẩm cần xóa</param>
        /// <returns>Kết quả xóa</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var command = new DeleteProductCommand { Id = id };
                await _dispatcher.DispatchAsync(command);
                return this.OkResponse("Xóa sản phẩm thành công");
            }
            catch (ArgumentException ex)
            {
                return this.ErrorResponse<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return this.ErrorResponse<object>($"Lỗi khi xóa sản phẩm: {ex.Message}");
            }
        }
    }
}
