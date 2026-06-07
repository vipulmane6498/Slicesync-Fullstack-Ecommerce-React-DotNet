using Microsoft.AspNetCore.Identity;

namespace SliceSync.Core.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FullName { get; set; }

        public bool? IsActive { get; set; }

        public string? JwtRefreshToken { get; set; }

        public DateTime? JwtRefreshTokenExpirationDateTime { get; set; }
    }
}
