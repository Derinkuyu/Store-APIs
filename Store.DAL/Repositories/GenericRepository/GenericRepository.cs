using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Store.DAL
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        /*------------------------------------------------------------------*/
        protected readonly AppDbContext _context;
        /*------------------------------------------------------------------*/
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
        /*------------------------------------------------------------------*/
        public async Task<IEnumerable<T>> GetAllGenericAsync(
            Expression<Func<T, bool>>? expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? includeProperties = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (expression is not null)
                query = query.Where(expression);

            if (includeProperties is not null)
            {
                foreach (var prop in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(prop.Trim());
            }

            if (orderBy is not null)
                query = orderBy(query);

            return await query.ToListAsync();
        }
        /*------------------------------------------------------------------*/
        public async Task<T?> GetByIdAsync(int id)
            => await _context.Set<T>().FindAsync(id);
        /*------------------------------------------------------------------*/
        public async Task AddAsync(T entity)
            => await _context.Set<T>().AddAsync(entity);
        /*------------------------------------------------------------------*/
        public void Update(T entity)
            => _context.Set<T>().Update(entity);
        /*------------------------------------------------------------------*/
        public void Delete(T entity)
            => _context.Set<T>().Remove(entity);
    }
}
