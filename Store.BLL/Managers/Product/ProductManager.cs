using AutoMapper;
using Store.Common;
using Store.DAL;

namespace Store.BLL
{
    public class ProductManager : IProductManager
    {
        /*------------------------------------------------------------------*/
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        /*------------------------------------------------------------------*/
        public ProductManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /*------------------------------------------------------------------*/
        public async Task<GeneralResult<PagedResult<ProductReadDto>>> GetProductsAsync(ProductFilterParameters filters)
        {
            var products = await _unitOfWork.ProductRepository.GetFilteredAsync(filters);
            var totalCount = await _unitOfWork.ProductRepository.GetFilteredCountAsync(filters);

            var items = _mapper.Map<IEnumerable<ProductReadDto>>(products);
            var result = new PagedResult<ProductReadDto>
            {
                Items = items,
                Metadata = new PaginationMetadata
                {
                    CurrentPage = filters.PageNumber,
                    PageSize = filters.PageSize,
                    TotalCount = totalCount
                }
            };

            return GeneralResult<PagedResult<ProductReadDto>>.SuccessResult(result);
        }
        /*------------------------------------------------------------------*/
        public async Task<GeneralResult<ProductReadDto>> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdWithCategoryAsync(id);
            if (product is null)
                return GeneralResult<ProductReadDto>.FailureResult($"Product with id {id} not found.");

            return GeneralResult<ProductReadDto>.SuccessResult(_mapper.Map<ProductReadDto>(product));
        }
        /*------------------------------------------------------------------*/
        public async Task<GeneralResult<ProductReadDto>> CreateProductAsync(ProductCreateDto dto)
        {
            // Business rule: category must exist
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(dto.CategoryId);
            if (category is null)
                return GeneralResult<ProductReadDto>.FailureResult("Category not found.");

            var product = _mapper.Map<Product>(dto);
            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            var created = await _unitOfWork.ProductRepository.GetByIdWithCategoryAsync(product.Id);
            return GeneralResult<ProductReadDto>.SuccessResult(
                _mapper.Map<ProductReadDto>(created), "Product created successfully.");
        }
        /*------------------------------------------------------------------*/
        public async Task<GeneralResult<ProductReadDto>> UpdateProductAsync(int id, ProductEditDto dto)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product is null)
                return GeneralResult<ProductReadDto>.FailureResult($"Product with id {id} not found.");

            _mapper.Map(dto, product);
            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();

            var updated = await _unitOfWork.ProductRepository.GetByIdWithCategoryAsync(id);
            return GeneralResult<ProductReadDto>.SuccessResult(
                _mapper.Map<ProductReadDto>(updated), "Product updated successfully.");
        }
        /*------------------------------------------------------------------*/
        public async Task<GeneralResult> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product is null)
                return GeneralResult.FailureResult($"Product with id {id} not found.");

            _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.SaveChangesAsync();

            return GeneralResult.SuccessResult("Product deleted successfully.");
        }
    }
}
