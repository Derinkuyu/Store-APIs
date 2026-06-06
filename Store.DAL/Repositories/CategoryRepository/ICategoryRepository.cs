using Store.Common;

namespace Store.DAL
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        /*------------------------------------------------------------------*/
        Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
    }
}
