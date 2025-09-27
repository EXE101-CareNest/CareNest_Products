using CareNest_Products.Application.Interfaces.CQRS.Queries;
using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Domain.Entities;

namespace CareNest_Products.Application.Features.Queries.GetById
{
    /// <summary>
    /// Handler cho query lấy sản phẩm theo ID
    /// </summary>
    public class GetByIdProductQueryHandler : IQueryHandler<GetByIdProductQuery, Product>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdProductQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> HandleAsync(GetByIdProductQuery query)
        {
            var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(query.Id);
            
            if (product == null)
            {
                throw new ArgumentException($"Không tìm thấy sản phẩm với ID: {query.Id}");
            }

            return product;
        }
    }
}
