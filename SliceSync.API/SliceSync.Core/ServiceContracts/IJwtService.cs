using SliceSync.Core.DTOs;
using SliceSync.Core.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.ServiceContracts
{
    public interface IJwtService
    {
        Task<AuthenticationResponseDTO> CreateJwtToken(ApplicationUser applicationUser);


        //This method extract the user details from supplied token
        ClaimsPrincipal? GetPrincipalfromJwtToken(string? token);
    }
}
