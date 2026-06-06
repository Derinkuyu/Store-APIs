using Store.BLL;
using Microsoft.AspNetCore.Mvc;

namespace Store.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /*------------------------------------------------------------------*/
        private readonly IAuthManager _authManager;
        /*------------------------------------------------------------------*/
        public AuthController(IAuthManager authManager)
        {
            _authManager = authManager;
        }
        /*------------------------------------------------------------------*/
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authManager.RegisterAsync(dto);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
        /*------------------------------------------------------------------*/
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authManager.LoginAsync(dto);
            if (!result.Success)
                return Unauthorized(result);
            return Ok(result);
        }
    }
}
