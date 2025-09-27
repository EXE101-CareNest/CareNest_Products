using CareNest_Products.Infrastructure.Persistences.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CareNest_Products.Infrastructure.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            try
            {
                await DatabaseSeeder.SeedAsync(context);
                app.Logger.LogInformation("Database seeded successfully");
            }
            catch (Exception ex)
            {
                app.Logger.LogError(ex, "An error occurred while seeding the database");
            }

            return app;
        }
    }
}
