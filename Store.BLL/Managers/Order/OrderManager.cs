using AutoMapper;
using Store.Common;
using Store.DAL;

namespace Store.BLL
{
    public class OrderManager : IOrderManager
    {
        /*------------------------------------------------------------------*/
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        /*------------------------------------------------------------------*/
        public OrderManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /*------------------------------------------------------------------*/
        public async Task<GeneralResult<OrderReadDto>> PlaceOrderAsync(string userId)
        {
            var cartItems = await _unitOfWork.CartRepository.GetUserCartAsync(userId);
            if (!cartItems.Any())
                return GeneralResult<OrderReadDto>.FailureResult("Your cart is empty.");

            var order = new Order
            {
                UserId = userId,
                Status = OrderStatus.Pending,
                OrderItems = cartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product?.Price ?? 0
                }).ToList()
            };
            order.TotalAmount = order.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity);

            await _unitOfWork.OrderRepository.AddAsync(order);

            // Clear cart
            foreach (var item in cartItems)
                _unitOfWork.CartRepository.Delete(item);

            await _unitOfWork.SaveChangesAsync();

            var created = await _unitOfWork.OrderRepository.GetOrderWithItemsAsync(order.Id, userId);
            return GeneralResult<OrderReadDto>.SuccessResult(_mapper.Map<OrderReadDto>(created), "Order placed successfully.");
        }
        /*------------------------------------------------------------------*/
        public async Task<GeneralResult<IEnumerable<OrderReadDto>>> GetUserOrdersAsync(string userId)
        {
            var orders = await _unitOfWork.OrderRepository.GetUserOrdersAsync(userId);
            return GeneralResult<IEnumerable<OrderReadDto>>.SuccessResult(_mapper.Map<IEnumerable<OrderReadDto>>(orders));
        }
        /*------------------------------------------------------------------*/
        public async Task<GeneralResult<OrderReadDto>> GetOrderByIdAsync(string userId, int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderWithItemsAsync(orderId, userId);
            if (order is null)
                return GeneralResult<OrderReadDto>.FailureResult($"Order with id {orderId} not found.");

            return GeneralResult<OrderReadDto>.SuccessResult(_mapper.Map<OrderReadDto>(order));
        }
    }
}
