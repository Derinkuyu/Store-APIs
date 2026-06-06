using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BLL;
using System.Security.Claims;

namespace Store.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        /*------------------------------------------------------------------*/
        private readonly ICartManager _cartManager;
        /*------------------------------------------------------------------*/
        public CartController(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }
        /*------------------------------------------------------------------*/
        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        /*------------------------------------------------------------------*/
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var result = await _cartManager.GetUserCartAsync(GetUserId());
            return Ok(result);
        }
        /*------------------------------------------------------------------*/
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartItemAddDto dto)
        {
            var result = await _cartManager.AddToCartAsync(GetUserId(), dto);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
        /*------------------------------------------------------------------*/
        [HttpPut]
        public async Task<IActionResult> UpdateCartItem([FromBody] CartItemUpdateDto dto)
        {
            var result = await _cartManager.UpdateCartItemAsync(GetUserId(), dto);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
        /*------------------------------------------------------------------*/
        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var result = await _cartManager.RemoveFromCartAsync(GetUserId(), productId);
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }
    }
}
