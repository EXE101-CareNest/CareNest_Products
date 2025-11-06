using CareNest_Products.Application;
using CareNest_Products.Application.Common.Options;
using CareNest_Products.Application.Interfaces.Services;
using CareNest_Products.Infrastructure;
using CareNest_Products.Infrastructure.Services;
using CareNest_Products.Infrastructure.Extensions;
using CareNest_Products.API.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });
builder.Services.AddControllers();
// CORS configuration - allow all
const string CorsPolicy = "FrontendPolicy";
builder.Services.AddCors(options =>
{
	options.AddPolicy(CorsPolicy, policy =>
	{
		policy
			.AllowAnyOrigin()
			.AllowAnyHeader()
			.AllowAnyMethod();
	});
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "CareNest Products API", 
        Version = "v1",
        Description = "API quản lý sản phẩm với Clean Architecture và CQRS pattern"
    });
    c.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });
});

// Add HTTP Context Accessor
builder.Services.AddHttpContextAccessor();

// Add HTTP Client
builder.Services.AddHttpClient();

// Configure Options
builder.Services.Configure<APIServiceOption>(options =>
{
    // Đọc từ appsettings trước
    builder.Configuration.GetSection("APIService").Bind(options);
    
    // Đọc từ environment variables và override nếu có
    // (Environment variables có priority cao hơn appsettings)
    var baseUrlShop = Environment.GetEnvironmentVariable("BASE_URL_SHOP");
    var baseUrlImage = Environment.GetEnvironmentVariable("BASE_URL_IMAGE");
    
    // Nếu giá trị từ appsettings là placeholder (${VAR}) hoặc empty, dùng env var
    if (!string.IsNullOrWhiteSpace(baseUrlShop) && 
        (string.IsNullOrWhiteSpace(options.BaseUrlShop) || options.BaseUrlShop.StartsWith("${")))
    {
        options.BaseUrlShop = baseUrlShop.Trim();
    }
    
    if (!string.IsNullOrWhiteSpace(baseUrlImage) && 
        (string.IsNullOrWhiteSpace(options.BaseUrlImage) || options.BaseUrlImage.StartsWith("${")))
    {
        options.BaseUrlImage = baseUrlImage.Trim();
    }
});

// Add Services
builder.Services.AddScoped<IAPIService, APIService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IImageService, ImageService>();

// Add MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(CareNest_Products.Application.ApplicationServiceRegistration).Assembly);
});

// Add Application services
builder.Services.AddApplicationServices();

// Add Infrastructure services
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Add Global Exception Handling Middleware
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

// Conditionally run migrations (for container/production)
var runMigrations = Environment.GetEnvironmentVariable("RUN_MIGRATIONS");
if (!string.IsNullOrWhiteSpace(runMigrations) && runMigrations.Equals("true", StringComparison.OrdinalIgnoreCase))
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<CareNest_Products.Infrastructure.Persistences.Database.ApplicationDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
var swaggerEnabled = app.Environment.IsDevelopment() || builder.Configuration.GetValue<bool>("Swagger:Enabled");
if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CareNest Products API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();

app.UseRouting();

// Enable CORS between routing and authorization
app.UseCors(CorsPolicy);

app.UseAuthorization();

app.MapControllers();

// Seed database
await app.SeedDatabaseAsync();

app.Run();
