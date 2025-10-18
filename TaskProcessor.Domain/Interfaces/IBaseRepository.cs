using System.Linq.Expressions;

namespace TaskProcessor.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        #region Public Methods

        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);

        #endregion Public Methods
    }
}
