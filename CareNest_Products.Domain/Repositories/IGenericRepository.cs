using System.Linq.Expressions;
using CareNest_Products.Domain.Commons;

namespace CareNest_Products.Domain.Repositories
{
    /// <summary>
    /// Generic repository interface
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IGenericRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Lấy tất cả entities
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Lấy entity theo ID
        /// </summary>
        Task<T?> GetByIdAsync(string id);

        /// <summary>
        /// Tìm entities theo điều kiện
        /// </summary>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Tìm entities với phân trang và sắp xếp
        /// </summary>
        Task<IEnumerable<TResult>> FindAsync<TResult>(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Expression<Func<T, TResult>>? selector = null,
            int? pageSize = null,
            int? pageIndex = null);

        /// <summary>
        /// Thêm entity mới
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Thêm nhiều entities
        /// </summary>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Cập nhật entity
        /// </summary>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Cập nhật nhiều entities
        /// </summary>
        Task UpdateRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Xóa entity
        /// </summary>
        Task DeleteAsync(T entity);

        /// <summary>
        /// Xóa entity theo ID
        /// </summary>
        Task DeleteAsync(string id);

        /// <summary>
        /// Xóa nhiều entities
        /// </summary>
        Task DeleteRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Kiểm tra entity có tồn tại không
        /// </summary>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Đếm số lượng entities
        /// </summary>
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    }
}
