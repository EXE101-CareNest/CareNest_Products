using CareNest_Products.Domain.Entities;
using CareNest_Products.Domain.Commons;

namespace CareNest_Products.Domain.Repositories
{
    /// <summary>
    /// Unit of Work interface để quản lý repositories và transactions
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Lấy repository cho entity type
        /// </summary>
        IGenericRepository<T> GetRepository<T>() where T : BaseEntity;

        /// <summary>
        /// Lưu thay đổi vào database
        /// </summary>
        Task<int> SaveAsync();

        /// <summary>
        /// Bắt đầu transaction
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commit transaction
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rollback transaction
        /// </summary>
        Task RollbackTransactionAsync();

        // Specific repositories
        IGenericRepository<Product> Products { get; }
        IGenericRepository<ProductCategory> ProductCategories { get; }
        IGenericRepository<ProductDetail> ProductDetails { get; }
    }
}
