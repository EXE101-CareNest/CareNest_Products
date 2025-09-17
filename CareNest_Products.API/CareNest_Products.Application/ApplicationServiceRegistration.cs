using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CareNest_Products.Application.Interfaces.CQRS;
using CareNest_Products.Application.UseCases;
using CareNest_Products.Application.Features.Commands.Create;
using CareNest_Products.Application.Features.Queries.GetAllPaging;
using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Application.Adapters;
using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Application.Interfaces.CQRS.Queries;

namespace CareNest_Products.Application
{
    /// <summary>
    /// Application service registration
    /// </summary>
    public static class ApplicationServiceRegistration
    {
        /// <summary>
        /// Đăng ký các services của Application layer
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationServiceRegistration).Assembly));

            // Use case dispatcher
            services.AddScoped<IUseCaseDispatcher, UseCaseDispatcher>();

            // Unit of Work adapter
            services.AddScoped<IUnitOfWork, UnitOfWorkAdapter>();

            // Command handlers
            services.AddScoped<ICommandHandler<CreateProductCommand, Domain.Entities.Product>, CreateProductCommandHandler>();

            // Query handlers
            services.AddScoped<IQueryHandler<GetAllProductsPagingQuery, Common.PageResult<ProductResponse>>, GetAllProductsPagingQueryHandler>();

            return services;
        }
    }
}
