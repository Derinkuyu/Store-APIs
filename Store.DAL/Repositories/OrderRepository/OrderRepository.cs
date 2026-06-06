using Microsoft.EntityFrameworkCore;

namespace Store.DAL
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        /*------------------------------------------------------------------*/
        public OrderRepository(AppDbContext context) : base(context)
        {
        }
        /*------------------------------------------------------------------*/
        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }
        /*------------------------------------------------------------------*/
        public async Task<Order?> GetOrderWithItemsAsync(int orderId, string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
        }
    }
}
