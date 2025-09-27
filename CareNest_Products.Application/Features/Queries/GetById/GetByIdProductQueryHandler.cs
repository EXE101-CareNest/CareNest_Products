using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Domain.Entities;
using MediatR;

namespace CareNest_Products.Application.Features.Queries.GetById
{
    /// <summary>
    /// Handler cho query lấy sản phẩm theo ID
    /// </summary>
    public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQuery, Product>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdProductQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> Handle(GetByIdProductQuery query, CancellationToken cancellationToken)
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
