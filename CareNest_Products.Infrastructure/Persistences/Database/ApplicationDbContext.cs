using Microsoft.EntityFrameworkCore;
using CareNest_Products.Domain.Entities;

namespace CareNest_Products.Infrastructure.Persistences.Database
{
    /// <summary>
    /// Application Database Context
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProductName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.ImgUrls).IsRequired();
                entity.Property(e => e.ProductCategoryId).IsRequired();
                entity.Property(e => e.Status).IsRequired().HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt);
                entity.Property(e => e.CreatedBy);
                entity.Property(e => e.UpdatedBy);

                // Foreign key relationship
                entity.HasOne(e => e.ProductCategory)
                    .WithMany(pc => pc.Products)
                    .HasForeignKey(e => e.ProductCategoryId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Indexes
                entity.HasIndex(e => e.ProductCategoryId);
                entity.HasIndex(e => e.ProductName);
                entity.HasIndex(e => e.Status);
            });

            // Configure ProductCategory entity
            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ShopId).IsRequired();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt);
                entity.Property(e => e.CreatedBy);
                entity.Property(e => e.UpdatedBy);

                // Indexes
                entity.HasIndex(e => e.ShopId);
                entity.HasIndex(e => e.Name);
            });

            // Configure ProductDetail entity
            modelBuilder.Entity<ProductDetail>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProductId).IsRequired();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.Status).IsRequired().HasDefaultValue(true);
                entity.Property(e => e.Discount);
                entity.Property(e => e.IsDefault).IsRequired().HasDefaultValue(false);
                entity.Property(e => e.ImgUrls).HasMaxLength(1000);
                entity.Property(e => e.QuantityInStock).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt);
                entity.Property(e => e.CreatedBy);
                entity.Property(e => e.UpdatedBy);

                // Foreign key relationship
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.ProductDetails)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Indexes
                entity.HasIndex(e => e.ProductId);
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.Price);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.IsDefault);
            });
        }
    }
}
