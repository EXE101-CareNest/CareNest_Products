using CareNest_Products.Application.Interfaces.CQRS.Queries;
using CareNest_Products.Domain.Entities;

namespace CareNest_Products.Application.Features.Queries.GetById
{
    /// <summary>
    /// Query lấy sản phẩm theo ID
    /// </summary>
    public class GetByIdProductQuery : IQuery<Product>
    {
        /// <summary>
        /// ID của sản phẩm
        /// </summary>
        public Guid Id { get; set; }
    }
}
