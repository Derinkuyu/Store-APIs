using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Common;
using Store.DAL;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Store.BLL
{
    public class AuthManager : IAuthManager
    {
        /*------------------------------------------------------------------*/
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        /*------------------------------------------------------------------*/
        public AuthManager(
            UserManager<AppUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        /*------------------------------------------------------------------*/
        public async Task<GeneralResult<AuthResultDto>> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser is not null)
                return GeneralResult<AuthResultDto>.FailureResult("Email is already registered.");

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.Email,
                Email = registerDto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                return GeneralResult<AuthResultDto>.FailureResult(
                    "Registration failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "User");

            var token = GenerateJwtToken(user, ["User"]);
            return GeneralResult<AuthResultDto>.SuccessResult(token, "Registration successful.");
        }
        /*------------------------------------------------------------------*/
        public async Task<GeneralResult<AuthResultDto>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return GeneralResult<AuthResultDto>.FailureResult("Invalid email or password.");

            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles);
            return GeneralResult<AuthResultDto>.SuccessResult(token, "Login successful.");
        }
        /*------------------------------------------------------------------*/
        private AuthResultDto GenerateJwtToken(AppUser user, IList<string> roles)
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(double.Parse(jwtSettings["DurationInDays"]!));

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.Name, user.DisplayName ?? user.UserName!),
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new AuthResultDto
            {
                DisplayName = user.DisplayName ?? string.Empty,
                Email = user.Email!,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expires
            };
        }
    }
}
