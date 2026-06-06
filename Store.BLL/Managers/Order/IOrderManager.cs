using Store.Common;

namespace Store.BLL
{
    public interface IOrderManager
    {
        /*------------------------------------------------------------------*/
        Task<GeneralResult<OrderReadDto>> PlaceOrderAsync(string userId);
        Task<GeneralResult<IEnumerable<OrderReadDto>>> GetUserOrdersAsync(string userId);
        Task<GeneralResult<OrderReadDto>> GetOrderByIdAsync(string userId, int orderId);
    }
}
