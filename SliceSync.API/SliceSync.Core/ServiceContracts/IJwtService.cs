using SliceSync.Core.DTOs;
using SliceSync.Core.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.ServiceContracts
{
    public interface IJwtService
    {
        AuthenticationResponseDTO CreateJwtToken(ApplicationUser applicationUser);
    }
}
