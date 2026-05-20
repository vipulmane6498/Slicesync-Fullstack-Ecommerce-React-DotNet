using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
