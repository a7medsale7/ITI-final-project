using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Training_ITI.Data;

namespace Training_ITI.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _set;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _set = context.Set<T>();
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _set.AsQueryable();
            foreach (var include in includes) query = query.Include(include);
            return query;
        }

        public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _set.AsQueryable();
            foreach (var include in includes) query = query.Include(include);
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _set.Where(predicate);
            foreach (var include in includes) query = query.Include(include);
            return await query.ToListAsync();
        }

        public async Task AddAsync(T entity) => await _set.AddAsync(entity);
        public void Update(T entity) => _set.Update(entity);

        public async Task DeleteAsync(int id)
        {
            var entity = await _set.FindAsync(id);
            if (entity != null) _set.Remove(entity);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) => _set.AnyAsync(predicate);

        public Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
            => predicate == null ? _set.CountAsync() : _set.CountAsync(predicate);

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
