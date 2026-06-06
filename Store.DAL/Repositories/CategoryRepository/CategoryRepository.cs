using Microsoft.EntityFrameworkCore;
using Store.Common;

namespace Store.DAL
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        /*------------------------------------------------------------------*/
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
        /*------------------------------------------------------------------*/
        public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
        {
            return await _context.Categories
                .AnyAsync(c => c.Name == name && (excludeId == null || c.Id != excludeId));
        }
    }
}
