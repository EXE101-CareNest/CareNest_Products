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
                // Shop 1: Pet Paradise
                new ProductCategory
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ShopId = "9d4e7f2a5b8c1e3d6f9a2b4c7e5d8f1a",
                    Name = "Thức ăn cho chó",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new ProductCategory
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ShopId = "9d4e7f2a5b8c1e3d6f9a2b4c7e5d8f1a",
                    Name = "Thức ăn cho mèo",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new ProductCategory
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ShopId = "9d4e7f2a5b8c1e3d6f9a2b4c7e5d8f1a",
                    Name = "Đồ chơi thú cưng",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Shop 2: Happy Pets
                new ProductCategory
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ShopId = "3f7a5c2b8e9d4f1a6b3c7e8d9f2a4b5c",
                    Name = "Phụ kiện thú cưng",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new ProductCategory
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ShopId = "3f7a5c2b8e9d4f1a6b3c7e8d9f2a4b5c",
                    Name = "Chăm sóc sức khỏe",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new ProductCategory
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ShopId = "3f7a5c2b8e9d4f1a6b3c7e8d9f2a4b5c",
                    Name = "Chuồng và giường",
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
                // Shop 1: Pet Paradise - Thức ăn cho chó (4 sản phẩm)
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[0].Id,
                    ProductName = "Royal Canin Adult Dog Food",
                    Description = "Thức ăn khô cao cấp cho chó trưởng thành, giàu protein và vitamin",
                    ImgUrls = "[\"https://example.com/royal-canin-dog.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[0].Id,
                    ProductName = "Pedigree Complete Nutrition",
                    Description = "Thức ăn khô cho chó với công thức cân bằng dinh dưỡng",
                    ImgUrls = "[\"https://example.com/pedigree-dog.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[0].Id,
                    ProductName = "Hill's Science Diet",
                    Description = "Thức ăn khoa học cho chó với công thức đặc biệt",
                    ImgUrls = "[\"https://example.com/hills-dog.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[0].Id,
                    ProductName = "Purina Pro Plan",
                    Description = "Thức ăn cao cấp cho chó với protein chất lượng cao",
                    ImgUrls = "[\"https://example.com/purina-dog.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Shop 1: Pet Paradise - Thức ăn cho mèo (3 sản phẩm)
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[1].Id,
                    ProductName = "Whiskas Cat Food",
                    Description = "Thức ăn ướt cho mèo với hương vị cá ngừ thơm ngon",
                    ImgUrls = "[\"https://example.com/whiskas-cat.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[1].Id,
                    ProductName = "Felix Cat Food",
                    Description = "Thức ăn ướt cho mèo với nhiều hương vị đa dạng",
                    ImgUrls = "[\"https://example.com/felix-cat.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[1].Id,
                    ProductName = "Sheba Cat Food",
                    Description = "Thức ăn cao cấp cho mèo với thịt thật",
                    ImgUrls = "[\"https://example.com/sheba-cat.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Shop 1: Pet Paradise - Đồ chơi thú cưng (3 sản phẩm)
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[2].Id,
                    ProductName = "Bóng tennis cho chó",
                    Description = "Bóng tennis cao su cho chó chơi đùa và tập thể dục",
                    ImgUrls = "[\"https://example.com/tennis-ball-dog.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[2].Id,
                    ProductName = "Xương giả cho chó",
                    Description = "Xương giả làm từ cao su tự nhiên, an toàn cho chó",
                    ImgUrls = "[\"https://example.com/dog-bone.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[2].Id,
                    ProductName = "Đồ chơi chuột cho mèo",
                    Description = "Đồ chơi chuột có tiếng kêu thu hút mèo",
                    ImgUrls = "[\"https://example.com/cat-mouse-toy.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Shop 2: Happy Pets - Phụ kiện thú cưng (4 sản phẩm)
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[3].Id,
                    ProductName = "Dây xích cho chó",
                    Description = "Dây xích da cao cấp, điều chỉnh được độ dài",
                    ImgUrls = "[\"https://example.com/dog-leash.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[3].Id,
                    ProductName = "Vòng cổ cho chó",
                    Description = "Vòng cổ da với khóa an toàn, nhiều màu sắc",
                    ImgUrls = "[\"https://example.com/dog-collar.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[3].Id,
                    ProductName = "Bát ăn cho thú cưng",
                    Description = "Bát ăn không gỉ, dễ vệ sinh",
                    ImgUrls = "[\"https://example.com/pet-bowl.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[3].Id,
                    ProductName = "Túi đựng thức ăn",
                    Description = "Túi đựng thức ăn tiện lợi khi đi du lịch",
                    ImgUrls = "[\"https://example.com/food-bag.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Shop 2: Happy Pets - Chăm sóc sức khỏe (3 sản phẩm)
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[4].Id,
                    ProductName = "Vitamin tổng hợp cho thú cưng",
                    Description = "Viên vitamin tổng hợp giúp tăng cường sức khỏe và hệ miễn dịch",
                    ImgUrls = "[\"https://example.com/pet-vitamins.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[4].Id,
                    ProductName = "Sữa tắm cho thú cưng",
                    Description = "Sữa tắm dịu nhẹ, không gây kích ứng da",
                    ImgUrls = "[\"https://example.com/pet-shampoo.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[4].Id,
                    ProductName = "Thuốc tẩy giun cho chó",
                    Description = "Thuốc tẩy giun an toàn và hiệu quả",
                    ImgUrls = "[\"https://example.com/deworming-medicine.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Shop 2: Happy Pets - Chuồng và giường (3 sản phẩm)
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[5].Id,
                    ProductName = "Giường ngủ cho chó",
                    Description = "Giường ngủ êm ái với chất liệu cotton cao cấp",
                    ImgUrls = "[\"https://example.com/dog-bed.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[5].Id,
                    ProductName = "Chuồng nhựa cho chó",
                    Description = "Chuồng nhựa chắc chắn, dễ vệ sinh",
                    ImgUrls = "[\"https://example.com/dog-crate.jpg\"]",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductCategoryId = categories[5].Id,
                    ProductName = "Nhà cho mèo",
                    Description = "Nhà cho mèo với thiết kế đẹp mắt",
                    ImgUrls = "[\"https://example.com/cat-house.jpg\"]",
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
                // Shop 1: Pet Paradise - Thức ăn cho chó
                // Royal Canin Adult Dog Food
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[0].Id,
                    Name = "Royal Canin Adult Dog Food 1kg",
                    Price = 250000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 100,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[0].Id,
                    Name = "Royal Canin Adult Dog Food 3kg",
                    Price = 650000,
                    Status = true,
                    IsDefault = false,
                    QuantityInStock = 50,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Pedigree Complete Nutrition
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[1].Id,
                    Name = "Pedigree Complete Nutrition 1.2kg",
                    Price = 180000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 80,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[1].Id,
                    Name = "Pedigree Complete Nutrition 3kg",
                    Price = 420000,
                    Status = true,
                    IsDefault = false,
                    QuantityInStock = 40,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Hill's Science Diet
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[2].Id,
                    Name = "Hill's Science Diet 1.8kg",
                    Price = 320000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 60,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Purina Pro Plan
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[3].Id,
                    Name = "Purina Pro Plan 1.4kg",
                    Price = 280000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 70,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Shop 1: Pet Paradise - Thức ăn cho mèo
                // Whiskas Cat Food
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[4].Id,
                    Name = "Whiskas Cat Food - Cá ngừ 400g",
                    Price = 45000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 80,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[4].Id,
                    Name = "Whiskas Cat Food - Thịt gà 400g",
                    Price = 45000,
                    Status = true,
                    IsDefault = false,
                    QuantityInStock = 60,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Felix Cat Food
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[5].Id,
                    Name = "Felix Cat Food - Cá hồi 400g",
                    Price = 48000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 50,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Sheba Cat Food
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[6].Id,
                    Name = "Sheba Cat Food - Thịt bò 400g",
                    Price = 55000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 45,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Shop 1: Pet Paradise - Đồ chơi thú cưng
                // Bóng tennis cho chó
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[7].Id,
                    Name = "Bóng tennis cao su cỡ vừa",
                    Price = 35000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 200,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Xương giả cho chó
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[8].Id,
                    Name = "Xương giả cao su cỡ lớn",
                    Price = 85000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 120,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Đồ chơi chuột cho mèo
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[9].Id,
                    Name = "Đồ chơi chuột có tiếng kêu",
                    Price = 65000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 150,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Shop 2: Happy Pets - Phụ kiện thú cưng
                // Dây xích cho chó
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[10].Id,
                    Name = "Dây xích da đen 1.2m",
                    Price = 180000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 40,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[10].Id,
                    Name = "Dây xích da nâu 1.5m",
                    Price = 200000,
                    Status = true,
                    IsDefault = false,
                    QuantityInStock = 30,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Vòng cổ cho chó
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[11].Id,
                    Name = "Vòng cổ da đen có chuông",
                    Price = 120000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 60,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Bát ăn cho thú cưng
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[12].Id,
                    Name = "Bát ăn không gỉ cỡ lớn",
                    Price = 95000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 80,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Túi đựng thức ăn
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[13].Id,
                    Name = "Túi đựng thức ăn 1kg",
                    Price = 75000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 100,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Shop 2: Happy Pets - Chăm sóc sức khỏe
                // Vitamin tổng hợp cho thú cưng
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[14].Id,
                    Name = "Vitamin tổng hợp hộp 60 viên",
                    Price = 320000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 25,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Sữa tắm cho thú cưng
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[15].Id,
                    Name = "Sữa tắm cho chó 500ml",
                    Price = 150000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 35,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Thuốc tẩy giun cho chó
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[16].Id,
                    Name = "Thuốc tẩy giun hộp 6 viên",
                    Price = 180000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 20,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Shop 2: Happy Pets - Chuồng và giường
                // Giường ngủ cho chó
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[17].Id,
                    Name = "Giường ngủ cỡ S (50x40cm)",
                    Price = 450000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 15,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[17].Id,
                    Name = "Giường ngủ cỡ L (80x60cm)",
                    Price = 650000,
                    Status = true,
                    IsDefault = false,
                    QuantityInStock = 10,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Chuồng nhựa cho chó
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[18].Id,
                    Name = "Chuồng nhựa cỡ M (60x45cm)",
                    Price = 380000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 12,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                // Nhà cho mèo
                new ProductDetail
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductId = products[19].Id,
                    Name = "Nhà cho mèo có cửa",
                    Price = 520000,
                    Status = true,
                    IsDefault = true,
                    QuantityInStock = 8,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                }
            };

            await context.ProductDetails.AddRangeAsync(productDetails);
        }
    }
}