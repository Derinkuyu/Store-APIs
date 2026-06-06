using AutoMapper;
using Store.Common;
using Store.DAL;

namespace Store.BLL
{
    public class CartManager : ICartManager
    {
        /*------------------------------------------------------------------*/
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        /*------------------------------------------------------------------*/
        public CartManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /*------------------------------------------------------------------*/
        public async Task<GeneralResult<CartReadDto>> GetUserCartAsync(string userId)
        {
            var items = await _unitOfWork.CartRepository.GetUserCartAsync(userId);
            var cartDto = new CartReadDto
            {
                Items = _mapper.Map<IEnumerable<CartItemReadDto>>(items)
            };
            return GeneralResult<CartReadDto>.SuccessResult(cartDto);
        }
        /*------------------------------------------------------------------*/
        public async Task<GeneralResult<CartItemReadDto>> AddToCartAsync(string userId, CartItemAddDto dto)
        {
            // Business rule: product must exist
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(dto.ProductId);
            if (product is null)
                return GeneralResult<CartItemReadDto>.FailureResult("Product not found.");

            var existingItem = await _unitOfWork.CartRepository.GetUserCartItemAsync(userId, dto.ProductId);
            if (existingItem is not null)
            {
                existingItem.Quantity += dto.Quantity;
                _unitOfWork.CartRepository.Update(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    UserId = userId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                };
                await _unitOfWork.CartRepository.AddAsync(newItem);
            }

            await _unitOfWork.SaveChangesAsync();

            // Reload with product info for the response
            var userCart = await _unitOfWork.CartRepository.GetUserCartAsync(userId);
            var cartItem = userCart.FirstOrDefault(i => i.ProductId == dto.ProductId);
            return GeneralResult<CartItemReadDto>.SuccessResult(
                _mapper.Map<CartItemReadDto>(cartItem), "Item added to cart.");
        }
        /*------------------------------------------------------------------*/
        public async Task<GeneralResult<CartItemReadDto>> UpdateCartItemAsync(string userId, CartItemUpdateDto dto)
        {
            var existingItem = await _unitOfWork.CartRepository.GetUserCartItemAsync(userId, dto.ProductId);
            if (existingItem is null)
                return GeneralResult<CartItemReadDto>.FailureResult("Cart item not found.");

            existingItem.Quantity = dto.Quantity;
            _unitOfWork.CartRepository.Update(existingItem);
            await _unitOfWork.SaveChangesAsync();

            var userCart = await _unitOfWork.CartRepository.GetUserCartAsync(userId);
            var cartItem = userCart.FirstOrDefault(i => i.ProductId == dto.ProductId);
            return GeneralResult<CartItemReadDto>.SuccessResult(
                _mapper.Map<CartItemReadDto>(cartItem), "Cart item updated.");
        }
        /*------------------------------------------------------------------*/
        public async Task<GeneralResult> RemoveFromCartAsync(string userId, int productId)
        {
            var existingItem = await _unitOfWork.CartRepository.GetUserCartItemAsync(userId, productId);
            if (existingItem is null)
                return GeneralResult.FailureResult("Cart item not found.");

            _unitOfWork.CartRepository.Delete(existingItem);
            await _unitOfWork.SaveChangesAsync();

            return GeneralResult.SuccessResult("Item removed from cart.");
        }
    }
}
