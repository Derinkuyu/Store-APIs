using Store.Common;

namespace Store.BLL
{
    public interface IAuthManager
    {
        /*------------------------------------------------------------------*/
        Task<GeneralResult<AuthResultDto>> RegisterAsync(RegisterDto registerDto);
        Task<GeneralResult<AuthResultDto>> LoginAsync(LoginDto loginDto);
    }
}
