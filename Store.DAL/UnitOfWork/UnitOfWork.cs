namespace Store.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        /*------------------------------------------------------------------*/
        private readonly AppDbContext _context;
        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }
        public ICartRepository CartRepository { get; }
        public IOrderRepository OrderRepository { get; }
        /*------------------------------------------------------------------*/
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            CategoryRepository = new CategoryRepository(context);
            ProductRepository = new ProductRepository(context);
            CartRepository = new CartRepository(context);
            OrderRepository = new OrderRepository(context);
        }
        /*------------------------------------------------------------------*/
        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
