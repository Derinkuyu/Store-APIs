namespace Store.BLL
{
    // Auth DTOs
    public class RegisterDto
    {
        /*------------------------------------------------------------------*/
        public required string DisplayName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class LoginDto
    {
        /*------------------------------------------------------------------*/
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class AuthResultDto
    {
        /*------------------------------------------------------------------*/
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
