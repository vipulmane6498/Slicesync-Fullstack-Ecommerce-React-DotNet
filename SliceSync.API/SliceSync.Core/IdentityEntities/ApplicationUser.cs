using Microsoft.AspNetCore.Identity;
using SliceSync.Core.Entities;

namespace SliceSync.Core.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FullName { get; set; }

        public bool? IsActive { get; set; }

        public string? JwtRefreshToken { get; set; }

        public DateTime? JwtRefreshTokenExpirationDateTime { get; set; }

        public List<Order>? Orders   { get; set; }
    }
}
