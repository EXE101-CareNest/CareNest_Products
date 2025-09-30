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

            // Tạo dữ liệu mẫu
            var products = await CreateSampleProducts(context);
            var categories = await CreateSampleCategories(context, products);
            await CreateSampleProductDetails(context, categories);

            await context.SaveChangesAsync();
        }

        private static async Task<List<Product>> CreateSampleProducts(ApplicationDbContext context)
        {
            var products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ShopId = "11111111-1111-1111-1111-111111111111", // Shop ID mẫu
                    ProductName = "iPhone 15 Pro Max",
                    Description = "Điện thoại iPhone 15 Pro Max với chip A17 Pro mạnh mẽ, camera 48MP và màn hình Super Retina XDR 6.7 inch",
                    ImgUrls = "[\"https://example.com/iphone15-1.jpg\", \"https://example.com/iphone15-2.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ShopId = "11111111-1111-1111-1111-111111111111",
                    ProductName = "Samsung Galaxy S24 Ultra",
                    Description = "Điện thoại Samsung Galaxy S24 Ultra với S Pen, camera 200MP và màn hình Dynamic AMOLED 2X 6.8 inch",
                    ImgUrls = "[\"https://example.com/samsung-s24-1.jpg\", \"https://example.com/samsung-s24-2.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ShopId = "22222222-2222-2222-2222-222222222222", // Shop khác
                    ProductName = "MacBook Pro M3",
                    Description = "Laptop MacBook Pro M3 với chip Apple M3, màn hình Liquid Retina XDR 14 inch và hiệu năng vượt trội",
                    ImgUrls = "[\"https://example.com/macbook-pro-1.jpg\", \"https://example.com/macbook-pro-2.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ShopId = "22222222-2222-2222-2222-222222222222",
                    ProductName = "Dell XPS 13",
                    Description = "Laptop Dell XPS 13 với Intel Core i7, màn hình 13.4 inch InfinityEdge và thiết kế siêu mỏng",
                    ImgUrls = "[\"https://example.com/dell-xps-1.jpg\", \"https://example.com/dell-xps-2.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                }
            };

            await context.Products.AddRangeAsync(products);
            return products;
        }

        private static async Task<List<ProductCategory>> CreateSampleCategories(ApplicationDbContext context, List<Product> products)
        {
            var categories = new List<ProductCategory>();

            // Categories cho iPhone 15 Pro Max
            categories.Add(new ProductCategory
            {
                Id = Guid.NewGuid().ToString("N"),
                ProductId = products[0].Id,
                Name = "Màu sắc",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            categories.Add(new ProductCategory
            {
                Id = Guid.NewGuid().ToString("N"),
                ProductId = products[0].Id,
                Name = "Dung lượng",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            // Categories cho Samsung Galaxy S24 Ultra
            categories.Add(new ProductCategory
            {
                Id = Guid.NewGuid().ToString("N"),
                ProductId = products[1].Id,
                Name = "Màu sắc",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            categories.Add(new ProductCategory
            {
                Id = Guid.NewGuid().ToString("N"),
                ProductId = products[1].Id,
                Name = "Dung lượng",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            // Categories cho MacBook Pro M3
            categories.Add(new ProductCategory
            {
                Id = Guid.NewGuid().ToString("N"),
                ProductId = products[2].Id,
                Name = "Cấu hình",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            categories.Add(new ProductCategory
            {
                Id = Guid.NewGuid().ToString("N"),
                ProductId = products[2].Id,
                Name = "Màu sắc",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            // Categories cho Dell XPS 13
            categories.Add(new ProductCategory
            {
                Id = Guid.NewGuid().ToString("N"),
                ProductId = products[3].Id,
                Name = "Cấu hình",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            categories.Add(new ProductCategory
            {
                Id = Guid.NewGuid().ToString("N"),
                ProductId = products[3].Id,
                Name = "Màu sắc",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            await context.ProductCategories.AddRangeAsync(categories);
            return categories;
        }

        private static async Task CreateSampleProductDetails(ApplicationDbContext context, List<ProductCategory> categories)
        {
            var productDetails = new List<ProductDetail>();

            // Product Details cho iPhone 15 Pro Max - Màu sắc
            productDetails.Add(new ProductDetail
            {
                Id = Guid.NewGuid().ToString("N"),
                CategoryId = categories[0].Id,
                Name = "Titanium Tự nhiên",
                Price = 29990000, // 29,990,000 VND
                Status = true,
                IsDefault = true,
                ImgUrls = "[\"https://example.com/iphone15-natural-1.jpg\"]",
                QuantityInStock = 50,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            productDetails.Add(new ProductDetail
            {
                Id = Guid.NewGuid().ToString("N"),
                CategoryId = categories[0].Id,
                Name = "Titanium Xanh",
                Price = 29990000,
                Status = true,
                IsDefault = false,
                ImgUrls = "[\"https://example.com/iphone15-blue-1.jpg\"]",
                QuantityInStock = 30,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            productDetails.Add(new ProductDetail
            {
                Id = Guid.NewGuid().ToString("N"),
                CategoryId = categories[0].Id,
                Name = "Titanium Trắng",
                Price = 29990000,
                Status = true,
                IsDefault = false,
                ImgUrls = "[\"https://example.com/iphone15-white-1.jpg\"]",
                QuantityInStock = 25,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            // Product Details cho iPhone 15 Pro Max - Dung lượng
            productDetails.Add(new ProductDetail
            {
                Id = Guid.NewGuid().ToString("N"),
                CategoryId = categories[1].Id,
                Name = "256GB",
                Price = 29990000,
                Status = true,
                IsDefault = true,
                ImgUrls = "[]",
                QuantityInStock = 100,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            productDetails.Add(new ProductDetail
            {
                Id = Guid.NewGuid().ToString("N"),
                CategoryId = categories[1].Id,
                Name = "512GB",
                Price = 33990000,
                Status = true,
                IsDefault = false,
                ImgUrls = "[]",
                QuantityInStock = 80,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            productDetails.Add(new ProductDetail
            {
                Id = Guid.NewGuid().ToString("N"),
                CategoryId = categories[1].Id,
                Name = "1TB",
                Price = 37990000,
                Status = true,
                IsDefault = false,
                ImgUrls = "[]",
                QuantityInStock = 60,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            // Product Details cho Samsung Galaxy S24 Ultra - Màu sắc
            productDetails.Add(new ProductDetail
            {
                Id = Guid.NewGuid().ToString("N"),
                CategoryId = categories[2].Id,
                Name = "Titanium Đen",
                Price = 27990000,
                Status = true,
                IsDefault = true,
                ImgUrls = "[\"https://example.com/samsung-s24-black-1.jpg\"]",
                QuantityInStock = 40,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            productDetails.Add(new ProductDetail
            {
                Id = Guid.NewGuid().ToString("N"),
                CategoryId = categories[2].Id,
                Name = "Titanium Vàng",
                Price = 27990000,
                Status = true,
                IsDefault = false,
                ImgUrls = "[\"https://example.com/samsung-s24-gold-1.jpg\"]",
                QuantityInStock = 35,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            // Product Details cho Samsung Galaxy S24 Ultra - Dung lượng
            productDetails.Add(new ProductDetail
            {
                Id = Guid.NewGuid().ToString("N"),
                CategoryId = categories[3].Id,
                Name = "256GB",
                Price = 27990000,
                Status = true,
                IsDefault = true,
                ImgUrls = "[]",
                QuantityInStock = 90,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            productDetails.Add(new ProductDetail
            {
                Id = Guid.NewGuid().ToString("N"),
                CategoryId = categories[3].Id,
                Name = "512GB",
                Price = 30990000,
                Status = true,
                IsDefault = false,
                ImgUrls = "[]",
                QuantityInStock = 70,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            // Product Details cho MacBook Pro M3 - Cấu hình
            productDetails.Add(new ProductDetail
            {
                Id = Guid.NewGuid().ToString("N"),
                CategoryId = categories[4].Id,
                Name = "M3 8-core CPU, 10-core GPU, 8GB RAM, 512GB SSD",
                Price = 42990000,
                Status = true,
                IsDefault = true,
                ImgUrls = "[\"https://example.com/macbook-m3-base-1.jpg\"]",
                QuantityInStock = 20,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            productDetails.Add(new ProductDetail
            {
                Id = Guid.NewGuid().ToString("N"),
                CategoryId = categories[4].Id,
                Name = "M3 8-core CPU, 10-core GPU, 16GB RAM, 1TB SSD",
                Price = 51990000,
                Status = true,
                IsDefault = false,
                ImgUrls = "[\"https://example.com/macbook-m3-upgraded-1.jpg\"]",
                QuantityInStock = 15,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            // Product Details cho MacBook Pro M3 - Màu sắc
            productDetails.Add(new ProductDetail
            {
                Id = Guid.NewGuid().ToString("N"),
                CategoryId = categories[5].Id,
                Name = "Xám Space",
                Price = 0, // Giá sẽ được tính từ cấu hình
                Status = true,
                IsDefault = true,
                ImgUrls = "[\"https://example.com/macbook-space-gray-1.jpg\"]",
                QuantityInStock = 50,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            productDetails.Add(new ProductDetail
            {
                Id = Guid.NewGuid().ToString("N"),
                CategoryId = categories[5].Id,
                Name = "Bạc",
                Price = 0,
                Status = true,
                IsDefault = false,
                ImgUrls = "[\"https://example.com/macbook-silver-1.jpg\"]",
                QuantityInStock = 45,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            // Product Details cho Dell XPS 13 - Cấu hình
            productDetails.Add(new ProductDetail
            {
                Id = Guid.NewGuid().ToString("N"),
                CategoryId = categories[6].Id,
                Name = "Intel Core i5, 8GB RAM, 512GB SSD",
                Price = 25990000,
                Status = true,
                IsDefault = true,
                ImgUrls = "[\"https://example.com/dell-xps-i5-1.jpg\"]",
                QuantityInStock = 25,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            productDetails.Add(new ProductDetail
            {
                Id = Guid.NewGuid().ToString("N"),
                CategoryId = categories[6].Id,
                Name = "Intel Core i7, 16GB RAM, 1TB SSD",
                Price = 32990000,
                Status = true,
                IsDefault = false,
                ImgUrls = "[\"https://example.com/dell-xps-i7-1.jpg\"]",
                QuantityInStock = 20,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            // Product Details cho Dell XPS 13 - Màu sắc
            productDetails.Add(new ProductDetail
            {
                Id = Guid.NewGuid().ToString("N"),
                CategoryId = categories[7].Id,
                Name = "Platinum Silver",
                Price = 0,
                Status = true,
                IsDefault = true,
                ImgUrls = "[\"https://example.com/dell-xps-silver-1.jpg\"]",
                QuantityInStock = 40,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            productDetails.Add(new ProductDetail
            {
                Id = Guid.NewGuid().ToString("N"),
                CategoryId = categories[7].Id,
                Name = "Frost White",
                Price = 0,
                Status = true,
                IsDefault = false,
                ImgUrls = "[\"https://example.com/dell-xps-white-1.jpg\"]",
                QuantityInStock = 35,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            });

            await context.ProductDetails.AddRangeAsync(productDetails);
        }
    }
}
