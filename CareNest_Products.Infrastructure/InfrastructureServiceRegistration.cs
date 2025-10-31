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

            // Database configuration (env-first + retry)
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var dbSection = configuration.GetSection("DatabaseSettings");
                var settings = new DatabaseSettings
                {
                    Ip = Environment.GetEnvironmentVariable("DB_HOST") ?? dbSection["Ip"],
                    Port = int.TryParse(Environment.GetEnvironmentVariable("DB_PORT"), out var envPort) ? envPort : (dbSection.GetValue<int?>("Port") ?? 5432),
                    User = Environment.GetEnvironmentVariable("DB_USER") ?? dbSection["User"],
                    Password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? dbSection["Password"],
                    Database = Environment.GetEnvironmentVariable("DB_NAME") ?? dbSection["Database"],
                    Pooling = bool.TryParse(Environment.GetEnvironmentVariable("DB_POOLING"), out var pooling) ? pooling : dbSection.GetValue<bool?>("Pooling"),
                    MaximumPoolSize = int.TryParse(Environment.GetEnvironmentVariable("DB_MAX_POOL_SIZE"), out var maxPool) ? maxPool : dbSection.GetValue<int?>("MaximumPoolSize"),
                    MinimumPoolSize = int.TryParse(Environment.GetEnvironmentVariable("DB_MIN_POOL_SIZE"), out var minPool) ? minPool : dbSection.GetValue<int?>("MinimumPoolSize"),
                    Timeout = int.TryParse(Environment.GetEnvironmentVariable("DB_TIMEOUT"), out var timeout) ? timeout : dbSection.GetValue<int?>("Timeout"),
                };

                var connectionString = settings.GetConnectionString();

                options.UseNpgsql(connectionString, b =>
                {
                    b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    b.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(5), errorCodesToAdd: null);
                });
            });

            // Repository registration
            services.AddScoped(typeof(Domain.Repositories.IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<Domain.Repositories.IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
