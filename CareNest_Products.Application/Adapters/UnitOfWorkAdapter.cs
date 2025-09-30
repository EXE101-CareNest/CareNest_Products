using CareNest_Products.Application.Interfaces.UOW;
using CareNest_Products.Domain.Repositories;
using CareNest_Products.Domain.Commons;
using DomainUOW = CareNest_Products.Domain.Repositories.IUnitOfWork;

namespace CareNest_Products.Application.Adapters
{
    /// <summary>
    /// Adapter để map giữa Domain và Application UOW
    /// </summary>
    public class UnitOfWorkAdapter : CareNest_Products.Application.Interfaces.UOW.IUnitOfWork
    {
        private readonly DomainUOW _domainUnitOfWork;

        public UnitOfWorkAdapter(DomainUOW domainUnitOfWork)
        {
            _domainUnitOfWork = domainUnitOfWork;
        }

        public CareNest_Products.Application.Interfaces.UOW.IGenericRepository<T> GetRepository<T>() where T : BaseEntity
        {
            var domainRepo = _domainUnitOfWork.GetRepository<T>();
            return new GenericRepositoryAdapter<T>(domainRepo);
        }

        public async Task<int> SaveAsync()
        {
            return await _domainUnitOfWork.SaveAsync();
        }

        public async Task BeginTransactionAsync()
        {
            await _domainUnitOfWork.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _domainUnitOfWork.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _domainUnitOfWork.RollbackTransactionAsync();
        }
    }

    /// <summary>
    /// Adapter để map giữa Domain và Application GenericRepository
    /// </summary>
    public class GenericRepositoryAdapter<T> : CareNest_Products.Application.Interfaces.UOW.IGenericRepository<T> where T : BaseEntity
    {
        private readonly Domain.Repositories.IGenericRepository<T> _domainRepository;

        public GenericRepositoryAdapter(Domain.Repositories.IGenericRepository<T> domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _domainRepository.GetAllAsync();
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            return await _domainRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> FindAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return await _domainRepository.FindAsync(predicate);
        }

        public async Task<IEnumerable<TResult>> FindAsync<TResult>(
            System.Linq.Expressions.Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            System.Linq.Expressions.Expression<Func<T, TResult>>? selector = null,
            int? pageSize = null,
            int? pageIndex = null)
        {
            return await _domainRepository.FindAsync(predicate, orderBy, selector, pageSize, pageIndex);
        }

        public async Task<T> AddAsync(T entity)
        {
            return await _domainRepository.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _domainRepository.AddRangeAsync(entities);
        }

        public async Task UpdateAsync(T entity)
        {
            await _domainRepository.UpdateAsync(entity);
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            await _domainRepository.UpdateRangeAsync(entities);
        }

        public async Task DeleteAsync(T entity)
        {
            await _domainRepository.DeleteAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _domainRepository.DeleteAsync(id);
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            await _domainRepository.DeleteRangeAsync(entities);
        }

        public async Task<bool> ExistsAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return await _domainRepository.ExistsAsync(predicate);
        }

        public async Task<int> CountAsync(System.Linq.Expressions.Expression<Func<T, bool>>? predicate = null)
        {
            return await _domainRepository.CountAsync(predicate);
        }
    }
}
