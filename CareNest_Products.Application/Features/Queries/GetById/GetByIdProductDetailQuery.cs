using CareNest_Products.Application.Features.Queries.GetAllPaging;
using MediatR;

namespace CareNest_Products.Application.Features.Queries.GetById
{
    /// <summary>
    /// Query lấy chi tiết sản phẩm theo ID
    /// </summary>
    public class GetByIdProductDetailQuery : IRequest<ProductDetailResponse>
    {
        /// <summary>
        /// ID của chi tiết sản phẩm
        /// </summary>
        public string Id { get; set; }
    }
}
