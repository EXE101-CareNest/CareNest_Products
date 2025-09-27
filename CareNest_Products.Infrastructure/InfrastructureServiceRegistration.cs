using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CareNest_Products.Domain.Repositories;
using CareNest_Products.Infrastructure.Persistences.Database;
using CareNest_Products.Infrastructure.Persistences.UOW;
using CareNest_Products.Infrastructure.Persistences.Repository;
using CareNest_Products.Infrastructure.Persistences.Configuration;

namespace CareNest_Products.Infrastructure
{
    /// <summary>
    /// Infrastructure service registration
    /// </summary>
    public static class InfrastructureServiceRegistration
    {
        /// <summary>
        /// Đăng ký các services của Infrastructure layer
        /// </summary>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure DatabaseSettings
            services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));

            // Database configuration
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var databaseSettings = configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();
                var connectionString = databaseSettings?.GetConnectionString() ?? 
                    throw new InvalidOperationException("Database settings not found");
                
                options.UseNpgsql(connectionString, 
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });

            // Repository registration
            services.AddScoped(typeof(Domain.Repositories.IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<Domain.Repositories.IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
