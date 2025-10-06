using CareNest_Products.Domain.Entities;
using CareNest_Products.Infrastructure.Persistences.Database;
using Microsoft.EntityFrameworkCore;

namespace CareNest_Products.Infrastructure.Persistences.Database
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Đảm bảo database được tạo
            await context.Database.EnsureCreatedAsync();

            // Kiểm tra xem đã có dữ liệu chưa
            if (await context.Products.AnyAsync())
            {
                return; // Đã có dữ liệu, không seed nữa
            }

            // Tạo dữ liệu mẫu theo cấu trúc mới: Shop -> ProductCategory -> Product -> ProductDetail
            var categories = await CreateSampleCategories(context);
            var products = await CreateSampleProducts(context, categories);
            await CreateSampleProductDetails(context, products);

            await context.SaveChangesAsync();
        }

        private static async Task<List<ProductCategory>> CreateSampleCategories(ApplicationDbContext context)
        {
            var categories = new List<ProductCategory>
            {
                new ProductCategory
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ShopId = "11111111-1111-1111-1111-111111111111",
                    Name = "Điện thoại",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new ProductCategory
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ShopId = "11111111-1111-1111-1111-111111111111",
                    Name = "Laptop",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                }
            };

            await context.ProductCategories.AddRangeAsync(categories);
            return categories;
        }

        private static async Task<List<Product>> CreateSampleProducts(ApplicationDbContext context, List<ProductCategory> categories)
        {
            var products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[0].Id, // Điện thoại
                    ProductName = "iPhone 15 Pro Max",
                    Description = "Điện thoại iPhone 15 Pro Max với chip A17 Pro mạnh mẽ",
                    ImgUrls = "[\"https://example.com/iphone15-1.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[1].Id, // Laptop
                    ProductName = "MacBook Pro M3",
                    Description = "Laptop MacBook Pro M3 với chip Apple M3",
                    ImgUrls = "[\"https://example.com/macbook-1.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                }
            };

            await context.Products.AddRangeAsync(products);
            return products;
        }

        private static async Task CreateSampleProductDetails(ApplicationDbContext context, List<Product> products)
        {
            var productDetails = new List<ProductDetail>
            {
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[0].Id,
                    Name = "iPhone 15 Pro Max 256GB",
                    Price = 29990000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 50,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[0].Id,
                    Name = "iPhone 15 Pro Max 512GB",
                    Price = 34990000,
                    Status = true,
                    IsDefault = false,
                    QuantityInStock = 30,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[1].Id,
                    Name = "MacBook Pro M3 14 inch",
                    Price = 45990000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 20,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                }
            };

            await context.ProductDetails.AddRangeAsync(productDetails);
        }
    }
}