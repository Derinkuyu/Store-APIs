using Microsoft.EntityFrameworkCore;
using Store.Common;

namespace Store.DAL
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        /*------------------------------------------------------------------*/
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        /*------------------------------------------------------------------*/
        // Builds the shared filtered IQueryable (no Include — count query doesn't need it)
        private IQueryable<Product> BuildFilteredQuery(ProductFilterParameters filters)
        {
            IQueryable<Product> query = _context.Products;

            /*── Filter by category ─────────────────────────────────────────*/
            if (filters.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == filters.CategoryId.Value);

            /*── Full-text search across Name and Description ───────────────*/
            if (!string.IsNullOrWhiteSpace(filters.Search))
                query = query.Where(p =>
                    p.Name.Contains(filters.Search) ||
                    (p.Description != null && p.Description.Contains(filters.Search)));

            /*── Price range ────────────────────────────────────────────────*/
            if (filters.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filters.MinPrice.Value);

            if (filters.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filters.MaxPrice.Value);

            return query;
        }

        /*------------------------------------------------------------------*/
        public async Task<IEnumerable<Product>> GetFilteredAsync(ProductFilterParameters filters)
        {
            IQueryable<Product> query = BuildFilteredQuery(filters);

            /*── Sorting ────────────────────────────────────────────────────*/
            query = (filters.SortBy?.ToLower(), filters.IsDescending) switch
            {
                ("price",       false) => query.OrderBy(p => p.Price),
                ("price",       true)  => query.OrderByDescending(p => p.Price),
                ("name",        false) => query.OrderBy(p => p.Name),
                ("name",        true)  => query.OrderByDescending(p => p.Name),
                ("createdat",   false) => query.OrderBy(p => p.CreatedAt),
                ("createdat",   true)  => query.OrderByDescending(p => p.CreatedAt),
                ("stock",       false) => query.OrderBy(p => p.Stock),
                ("stock",       true)  => query.OrderByDescending(p => p.Stock),
                _                      => query.OrderBy(p => p.Id)   // default order
            };

            /*── Eager-load category (after sorting, before paging) ─────────*/
            var includedQuery = query.Include(p => p.Category);

            /*── Pagination ─────────────────────────────────────────────────*/
            var skip = (filters.PageNumber - 1) * filters.PageSize;
            return await includedQuery.Skip(skip).Take(filters.PageSize).ToListAsync();
        }

        /*------------------------------------------------------------------*/
        public async Task<int> GetFilteredCountAsync(ProductFilterParameters filters)
        {
            // No Include needed for count — cheaper query
            return await BuildFilteredQuery(filters).CountAsync();
        }

        /*------------------------------------------------------------------*/
        public async Task<Product?> GetByIdWithCategoryAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
