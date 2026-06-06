using Store.Common;

namespace Store.BLL
{
    public interface IProductManager
    {
        /*------------------------------------------------------------------*/
        Task<GeneralResult<PagedResult<ProductReadDto>>> GetProductsAsync(ProductFilterParameters filters);
        Task<GeneralResult<ProductReadDto>> GetProductByIdAsync(int id);
        Task<GeneralResult<ProductReadDto>> CreateProductAsync(ProductCreateDto dto);
        Task<GeneralResult<ProductReadDto>> UpdateProductAsync(int id, ProductEditDto dto);
        Task<GeneralResult> DeleteProductAsync(int id);
    }
}
