using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.IdentityModule
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;
        public Address Address { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    }
}
