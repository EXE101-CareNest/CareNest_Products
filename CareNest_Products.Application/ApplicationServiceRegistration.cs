using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CareNest_Products.Application.Interfaces.CQRS;
using CareNest_Products.Application.UseCases;
using CareNest_Products.Application.Features.Commands.Create;
using CareNest_Products.Application.Features.Commands.Update;
using CareNest_Products.Application.Features.Commands.Delete;
using CareNest_Products.Application.Features.Queries.GetAllPaging;
using CareNest_Products.Application.Features.Queries.GetById;
using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Application.Adapters;
using CareNest_Products.Application.Interfaces.CQRS.Commands;
using CareNest_Products.Application.Interfaces.CQRS.Queries;
using CareNest_Products.Application.Interfaces.Services;
using CareNest_Products.Domain.Entities;

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
            // Unit of Work adapter
            services.AddScoped<IUnitOfWork, UnitOfWorkAdapter>();

            // UseCase dispatcher
            services.AddScoped<IUseCaseDispatcher, UseCaseDispatcher>();

            // Register command handlers
            services.AddScoped<ICommandHandler<CreateProductCommand, Product>, CreateProductCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateProductCommand, Product>, UpdateProductCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteProductCommand>, DeleteProductCommandHandler>();
            services.AddScoped<ICommandHandler<CreateProductCategoryCommand, ProductCategory>, CreateProductCategoryCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateProductCategoryCommand, ProductCategory>, UpdateProductCategoryCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteProductCategoryCommand>, DeleteProductCategoryCommandHandler>();
            services.AddScoped<ICommandHandler<CreateProductDetailCommand, ProductDetail>, CreateProductDetailCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateProductDetailCommand, ProductDetail>, UpdateProductDetailCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteProductDetailCommand>, DeleteProductDetailCommandHandler>();

            return services;
        }
    }
}
