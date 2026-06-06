using Microsoft.AspNetCore.Identity;

namespace Store.DAL
{
    public class AppUser : IdentityUser
    {
        /*------------------------------------------------------------------*/
        public string? DisplayName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
