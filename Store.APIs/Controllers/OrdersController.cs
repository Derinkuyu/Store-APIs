using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BLL;
using System.Security.Claims;

namespace Store.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        /*------------------------------------------------------------------*/
        private readonly IOrderManager _orderManager;
        /*------------------------------------------------------------------*/
        public OrdersController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }
        /*------------------------------------------------------------------*/
        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        /*------------------------------------------------------------------*/
        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var result = await _orderManager.PlaceOrderAsync(GetUserId());
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
        /*------------------------------------------------------------------*/
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var result = await _orderManager.GetUserOrdersAsync(GetUserId());
            return Ok(result);
        }
        /*------------------------------------------------------------------*/
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var result = await _orderManager.GetOrderByIdAsync(GetUserId(), id);
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }
    }
}
