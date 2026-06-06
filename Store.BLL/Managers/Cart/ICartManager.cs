using Store.Common;

namespace Store.BLL
{
    public interface ICartManager
    {
        /*------------------------------------------------------------------*/
        Task<GeneralResult<CartReadDto>> GetUserCartAsync(string userId);
        Task<GeneralResult<CartItemReadDto>> AddToCartAsync(string userId, CartItemAddDto dto);
        Task<GeneralResult<CartItemReadDto>> UpdateCartItemAsync(string userId, CartItemUpdateDto dto);
        Task<GeneralResult> RemoveFromCartAsync(string userId, int productId);
    }
}
