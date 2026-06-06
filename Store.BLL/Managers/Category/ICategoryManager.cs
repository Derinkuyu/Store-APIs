using Store.Common;

namespace Store.BLL
{
    public interface ICategoryManager
    {
        /*------------------------------------------------------------------*/
        Task<GeneralResult<IEnumerable<CategoryReadDto>>> GetAllCategoriesAsync();
        Task<GeneralResult<CategoryReadDto>> GetCategoryByIdAsync(int id);
        Task<GeneralResult<CategoryReadDto>> CreateCategoryAsync(CategoryCreateDto dto);
        Task<GeneralResult<CategoryReadDto>> UpdateCategoryAsync(int id, CategoryEditDto dto);
        Task<GeneralResult> DeleteCategoryAsync(int id);
    }
}
