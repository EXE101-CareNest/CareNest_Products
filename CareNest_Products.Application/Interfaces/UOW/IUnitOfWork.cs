using CareNest_Products.Domain.Repositories;

using CareNest_Products.Domain.Commons;

namespace CareNest_Products.Application.Interfaces.UOW
{
    /// <summary>
    /// Unit of Work interface cho Application layer
    /// </summary>
    public interface IUnitOfWork
    {
        IGenericRepository<T> GetRepository<T>() where T : BaseEntity;
        Task<int> SaveAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
