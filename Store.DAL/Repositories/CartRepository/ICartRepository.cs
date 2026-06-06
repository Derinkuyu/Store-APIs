namespace Store.DAL
{
    public interface ICartRepository : IGenericRepository<CartItem>
    {
        /*------------------------------------------------------------------*/
        Task<IEnumerable<CartItem>> GetUserCartAsync(string userId);
        Task<CartItem?> GetUserCartItemAsync(string userId, int productId);
    }
}
