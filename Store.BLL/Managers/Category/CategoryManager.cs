using AutoMapper;
using Store.Common;
using Store.DAL;

namespace Store.BLL
{
    public class CategoryManager : ICategoryManager
    {
        /*------------------------------------------------------------------*/
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        /*------------------------------------------------------------------*/
        public CategoryManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /*------------------------------------------------------------------*/
        public async Task<GeneralResult<IEnumerable<CategoryReadDto>>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllGenericAsync();
            var result = _mapper.Map<IEnumerable<CategoryReadDto>>(categories);
            return GeneralResult<IEnumerable<CategoryReadDto>>.SuccessResult(result);
        }
        /*------------------------------------------------------------------*/
        public async Task<GeneralResult<CategoryReadDto>> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category is null)
                return GeneralResult<CategoryReadDto>.FailureResult($"Category with id {id} not found.");

            return GeneralResult<CategoryReadDto>.SuccessResult(_mapper.Map<CategoryReadDto>(category));
        }
        /*------------------------------------------------------------------*/
        public async Task<GeneralResult<CategoryReadDto>> CreateCategoryAsync(CategoryCreateDto dto)
        {
            // Business rule: unique name check (not a simple format rule — stays in Manager)
            if (await _unitOfWork.CategoryRepository.ExistsByNameAsync(dto.Name))
                return GeneralResult<CategoryReadDto>.FailureResult("Category name already exists.");

            var category = _mapper.Map<Category>(dto);
            await _unitOfWork.CategoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return GeneralResult<CategoryReadDto>.SuccessResult(
                _mapper.Map<CategoryReadDto>(category), "Category created successfully.");
        }
        /*------------------------------------------------------------------*/
        public async Task<GeneralResult<CategoryReadDto>> UpdateCategoryAsync(int id, CategoryEditDto dto)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category is null)
                return GeneralResult<CategoryReadDto>.FailureResult($"Category with id {id} not found.");

            _mapper.Map(dto, category);
            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();

            return GeneralResult<CategoryReadDto>.SuccessResult(
                _mapper.Map<CategoryReadDto>(category), "Category updated successfully.");
        }
        /*------------------------------------------------------------------*/
        public async Task<GeneralResult> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category is null)
                return GeneralResult.FailureResult($"Category with id {id} not found.");

            _unitOfWork.CategoryRepository.Delete(category);
            await _unitOfWork.SaveChangesAsync();

            return GeneralResult.SuccessResult("Category deleted successfully.");
        }
    }
}
