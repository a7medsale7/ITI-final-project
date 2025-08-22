using System.Linq.Expressions;

namespace Training_ITI.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity);
        void Update(T entity);
        Task DeleteAsync(int id);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        Task SaveChangesAsync();
    }
}
