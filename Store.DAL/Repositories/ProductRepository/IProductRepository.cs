using Store.Common;

namespace Store.DAL
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        /*------------------------------------------------------------------*/
        Task<IEnumerable<Product>> GetFilteredAsync(ProductFilterParameters filters);
        Task<int> GetFilteredCountAsync(ProductFilterParameters filters);
        Task<Product?> GetByIdWithCategoryAsync(int id);
    }
}
