using CareNest_Products.Application.Features.Queries.GetAllPaging;
using MediatR;

namespace CareNest_Products.Application.Features.Queries.GetById
{
    /// <summary>
    /// Query lấy danh mục sản phẩm theo ID
    /// </summary>
    public class GetByIdProductCategoryQuery : IRequest<ProductCategoryResponse>
    {
        /// <summary>
        /// ID của danh mục sản phẩm
        /// </summary>
        public string Id { get; set; }
    }
}
