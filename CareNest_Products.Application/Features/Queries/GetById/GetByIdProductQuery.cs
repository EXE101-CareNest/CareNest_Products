using CareNest_Products.Domain.Entities;
using CareNest_Products.Application.Features.Queries.GetById;
using MediatR;

namespace CareNest_Products.Application.Features.Queries.GetById
{
    /// <summary>
    /// Query lấy sản phẩm theo ID
    /// </summary>
public class GetByIdProductQuery : IRequest<ProductWithCategoriesResponse>
    {
        /// <summary>
        /// ID của sản phẩm
        /// </summary>
        public string Id { get; set; }
    }
}
