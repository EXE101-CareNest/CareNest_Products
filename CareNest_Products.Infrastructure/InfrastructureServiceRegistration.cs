using Microsoft.EntityFrameworkCore;
using System;
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

            // Database configuration (prefer DATABASE_URL, fallback to env-first fields + retry)
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var dbSection = configuration.GetSection("DatabaseSettings");

                var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
                string connectionString;
                if (!string.IsNullOrWhiteSpace(databaseUrl))
                {
                    var uri = new Uri(databaseUrl);
                    var userInfoParts = uri.UserInfo.Split(':', 2);
                    var username = userInfoParts.Length > 0 ? Uri.UnescapeDataString(userInfoParts[0]) : string.Empty;
                    var password = userInfoParts.Length > 1 ? Uri.UnescapeDataString(userInfoParts[1]) : string.Empty;
                    var host = uri.Host;
                    var port = uri.Port;
                    var database = uri.AbsolutePath.TrimStart('/');

                    var pooling = bool.TryParse(Environment.GetEnvironmentVariable("DB_POOLING"), out var poolingEnv)
                        ? $";Pooling={(poolingEnv ? "true" : "false")}"
                        : (dbSection.GetValue<bool?>("Pooling") is bool p ? $";Pooling={(p ? "true" : "false")}" : string.Empty);
                    var maxPool = int.TryParse(Environment.GetEnvironmentVariable("DB_MAX_POOL_SIZE"), out var maxPoolEnv)
                        ? $";Maximum Pool Size={maxPoolEnv}"
                        : (dbSection.GetValue<int?>("MaximumPoolSize") is int mp ? $";Maximum Pool Size={mp}" : string.Empty);
                    var minPool = int.TryParse(Environment.GetEnvironmentVariable("DB_MIN_POOL_SIZE"), out var minPoolEnv)
                        ? $";Minimum Pool Size={minPoolEnv}"
                        : (dbSection.GetValue<int?>("MinimumPoolSize") is int mi ? $";Minimum Pool Size={mi}" : string.Empty);
                    var timeout = int.TryParse(Environment.GetEnvironmentVariable("DB_TIMEOUT"), out var timeoutEnv)
                        ? $";Timeout={timeoutEnv}"
                        : (dbSection.GetValue<int?>("Timeout") is int to ? $";Timeout={to}" : string.Empty);

                    connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}" + pooling + maxPool + minPool + timeout;
                }
                else
                {
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

                    connectionString = settings.GetConnectionString();
                }

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
