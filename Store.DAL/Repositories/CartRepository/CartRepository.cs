using Microsoft.EntityFrameworkCore;

namespace Store.DAL
{
    public class CartRepository : GenericRepository<CartItem>, ICartRepository
    {
        /*------------------------------------------------------------------*/
        public CartRepository(AppDbContext context) : base(context)
        {
        }
        /*------------------------------------------------------------------*/
        public async Task<IEnumerable<CartItem>> GetUserCartAsync(string userId)
        {
            return await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }
        /*------------------------------------------------------------------*/
        public async Task<CartItem?> GetUserCartItemAsync(string userId, int productId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
        }
    }
}
